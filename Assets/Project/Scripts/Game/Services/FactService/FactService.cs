using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Clicker.Core;
using Clicker.Core.RequestQueue;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace Clicker.Game.Services
{
	public class FactService : IFactService
	{
		public IReadOnlyReactiveProperty<List<DogFact>> DogFacts => _dogFacts;
		public IReadOnlyReactiveProperty<DogFactDetails> SelectedFactDetails => _selectedFactDetails;

		private readonly FactConfig _factConfig;
		private readonly ReactiveProperty<List<DogFact>> _dogFacts = new();
		private readonly ReactiveProperty<DogFactDetails> _selectedFactDetails = new();
		private readonly IRequestQueue _requestQueue;
		private string _currentFactsRequestId;
		private string _currentFactRequestId;

		public FactService(IRequestQueue requestQueue, IGameConfigurationService gameConfigurationService)
		{
			_factConfig = gameConfigurationService.GameConfiguration.FactConfig;
			_requestQueue = requestQueue;
		}

		public void CancelDogFactsRequest()
		{
			if (_currentFactsRequestId != null)
			{
				_requestQueue.CancelRequest(_currentFactsRequestId);
			}
		}

		public void CancelDogFactDetailsRequest()
		{
			if (_currentFactRequestId != null)
			{
				_requestQueue.CancelRequest(_currentFactRequestId);
			}
		}

		public void FetchDogFacts()
		{
			if (_currentFactsRequestId != null)
			{
				_requestQueue.CancelRequest(_currentFactsRequestId);
			}

			_currentFactsRequestId = $"dog_facts";
			_requestQueue.AddRequest(async () =>
			{
				var url = _factConfig.ApiKey;
				var response = await FetchData(url);
				if (!string.IsNullOrEmpty(response))
				{
					try
					{
						var apiResponse = JsonConvert.DeserializeObject<DogApiResponse>(response);
						if (apiResponse?.data != null)
						{
							var facts = apiResponse.data.Select(d => new DogFact
							{
								id = d.id,
								name = d.attributes.name
							}).ToList();

							_dogFacts.Value = facts;
						}
					}
					catch (Exception ex)
					{
						Debug.LogError($"Error parsing dog facts: {ex.Message}");
					}
				}
			}, _currentFactsRequestId);
		}

		public void FetchDogFactDetails(string id, Action<DogFactDetails> onComplete)
		{
			if (_currentFactRequestId != null)
			{
				_requestQueue.ClearQueue();
			}

			_currentFactRequestId = $"dog_fact_{id}";

			_requestQueue.AddRequest(async () =>
			{
				var url = $"https://dogapi.dog/api/v2/breeds/{id}";


				foreach (var dogFact in _dogFacts.Value)
				{
					dogFact.IsLoading.Value = false;
				}

				var dogFactNext = _dogFacts.Value.FirstOrDefault(x => x.id == id);

				if (dogFactNext != null)
				{
					dogFactNext.IsLoading.Value = true;
				}

				var response = await FetchData(url);


				if (!string.IsNullOrEmpty(response))
				{
					try
					{
						var apiResponse = JsonConvert.DeserializeObject<DogApiResponseId>(response);
						if (apiResponse != null)
						{
							var details = new DogFactDetails
							{
								id = apiResponse.data.id,
								name = apiResponse.data.attributes.name,
								description = apiResponse.data.attributes.description
							};

							_selectedFactDetails.Value = details;

							if (dogFactNext != null)
							{
								dogFactNext.IsLoading.Value = false;
							}

							onComplete?.Invoke(_selectedFactDetails.Value);
						}
					}
					catch (Exception ex)
					{
						Debug.LogError($"Error parsing dog fact details: {ex.Message}");
					}
				}
			}, _currentFactRequestId);
		}

		private async UniTask<string> FetchData(string url)
		{
			using var client = new HttpClient();
			client.DefaultRequestHeaders.Add("User-Agent", "YourAppName (your.email@example.com)");
			try
			{
				var response = await client.GetStringAsync(url);
				return response;
			}
			catch (HttpRequestException ex)
			{
				Debug.LogError($"HTTP Request failed: {ex.Message}");
				return string.Empty;
			}
		}
	}
}