using UnityEngine;

namespace Clicker.Game.Services
{
	public class WeatherData
	{
		public readonly WeatherResponse WeatherResponse;
		public readonly Sprite Sprite;

		public WeatherData(WeatherResponse weatherResponse, Sprite sprite)
		{
			WeatherResponse = weatherResponse;
			Sprite = sprite;
		}
	}
}