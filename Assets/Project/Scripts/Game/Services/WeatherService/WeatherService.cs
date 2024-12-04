using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Clicker.Core;
using Clicker.Core.RequestQueue;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Clicker.Game.Services
{
	public class WeatherService : IWeatherService
	{
		public IReadOnlyReactiveProperty<WeatherData> WeatherInfo => _weatherData;

		private ReactiveProperty<WeatherData> _weatherData = new();
		private readonly IRequestQueue _requestQueue;
		private readonly WeatherConfig _weatherConfig;
		private CancellationTokenSource _cancellationTokenSource;

		public WeatherService(IRequestQueue queue, IGameConfigurationService gameConfigurationService)
		{
			_requestQueue = queue;
			_weatherConfig = gameConfigurationService.GameConfiguration.WeatherConfig;
		}

		public void StartWeatherUpdates()
		{
			StopWeatherUpdates();

			_cancellationTokenSource = new CancellationTokenSource();

			UniTask.Create(async () =>
			{
				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					Debug.Log("New Weather Request");
					GetWeatherData();
					await UniTask.Delay(TimeSpan.FromSeconds(_weatherConfig.TimeRequest),
					                    cancellationToken: _cancellationTokenSource.Token);
				}
			});
		}

		public void StopWeatherUpdates()
		{
			_cancellationTokenSource?.Cancel();
			_requestQueue.CancelRequest("weather_request");
		}

		private async Task<string> FetchWeatherData(string url)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("User-Agent", "YourAppName (your.email@example.com)");
				try
				{
					var response = await client.GetStringAsync(url);
					return response;
				}
				catch (HttpRequestException ex)
				{
					Debug.LogError($"HTTP Request failed: {ex.Message}");
					return null;
				}
			}
		}

		private void GetWeatherData()
		{
			var requestId = "weather_request";
			_requestQueue.AddRequest(async () =>
			{
				var url = _weatherConfig.ApiKey;
				var response = await FetchWeatherData(url);
				ProcessWeatherData(response);
			}, requestId);
		}

		private async void ProcessWeatherData(string response)
		{
			if (string.IsNullOrEmpty(response))
			{
				Debug.LogWarning("Empty weather response");
				_weatherData.Value = null;
				return;
			}

			try
			{
				var dto = JsonConvert.DeserializeObject<WeatherResponse>(response);
				if (dto != null)
				{
					var icon = await LoadWeatherIconAsync(dto.properties.periods[0].icon);
					_weatherData.Value = new WeatherData(dto, icon ?? null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Error processing weather data: {ex.Message}");
				_weatherData.Value = null;
			}
		}

		private async UniTask<Sprite> LoadWeatherIconAsync(string iconUrl)
		{
			using (var request = UnityWebRequestTexture.GetTexture(iconUrl))
			{
				await request.SendWebRequest();
				if (request.result == UnityWebRequest.Result.Success)
				{
					var texture = DownloadHandlerTexture.GetContent(request);
					return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
				}
				else
				{
					Debug.LogError($"Failed to load weather icon: {request.error}");
					return null;
				}
			}
		}
	}
}