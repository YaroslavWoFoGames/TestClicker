namespace Clicker.Game.Services
{
	[System.Serializable]
	public class WeatherResponse
	{
		public Properties properties;

		[System.Serializable]
		public class Properties
		{
			public Period[] periods;
		}

		[System.Serializable]
		public class Period
		{
			public string name;
			public int temperature;
			public string temperatureUnit;
			public string icon;
			public string shortForecast;
		}
	}
}