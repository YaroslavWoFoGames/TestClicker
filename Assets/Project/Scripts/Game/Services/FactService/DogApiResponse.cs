using System;
using System.Collections.Generic;
using UniRx;

namespace Clicker.Game.Services
{
	[Serializable]
	public class DogFact
	{
		public string id;
		public string name;
		public ReactiveProperty<bool> IsLoading { get; private set; } = new(false);
	}

	[Serializable]
	public class DogFactDetails
	{
		public string id;
		public string name;
		public string description;
	}

	[Serializable]
	public class DogApiResponse
	{
		public List<DogFactData> data;
	}

	[Serializable]
	public class DogApiResponseId
	{
		public DogFactData data;
	}

	[Serializable]
	public class DogFactData
	{
		public string id;
		public string type;
		public DogFactAttributes attributes;
	}

	[Serializable]
	public class DogFactAttributes
	{
		public string name;
		public string description;
		public bool hypoallergenic;
	}
}