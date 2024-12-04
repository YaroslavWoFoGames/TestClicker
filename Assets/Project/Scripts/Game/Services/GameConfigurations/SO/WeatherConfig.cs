using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(WeatherConfig),
		                fileName = nameof(WeatherConfig))]
	public class WeatherConfig : Config
	{
		[field: SerializeField]
		public string ApiKey { get; private set; } = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

		[field: SerializeField] public float TimeRequest { get; private set; } = 5;
	}
}