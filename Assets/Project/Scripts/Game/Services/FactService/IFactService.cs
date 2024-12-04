using System;
using System.Collections.Generic;
using Clicker.Game.Services;
using UniRx;

namespace Clicker.Core
{
	public interface IFactService
	{
		void CancelDogFactsRequest();
		void CancelDogFactDetailsRequest();
		IReadOnlyReactiveProperty<List<DogFact>> DogFacts { get; }
		IReadOnlyReactiveProperty<DogFactDetails> SelectedFactDetails { get; }
		void FetchDogFacts();
		void FetchDogFactDetails(string id, Action<DogFactDetails> onComplete);
	}
}