using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurationsMain, fileName = nameof(GameConfiguration))]
	public class GameConfiguration : Config
	{
		[field: SerializeField] public WeatherConfig WeatherConfig { get; private set; } = null!;
		[field: SerializeField] public CurrencyConfig CurrencyConfig { get; private set; } = null!;
		[field: SerializeField] public AudioConfig AudioConfig { get; private set; } = null!;
		[field: SerializeField] public FactConfig FactConfig { get; private set; } = null!;
		[field: SerializeField] public EnergyConfig EnergyConfig { get; private set; } = null!;
		[field: SerializeField] public UIConfig UIConfig { get; private set; } = null!;
		[field: SerializeField] public SceneConfig SceneConfig { get; private set; } = null!;
	}
}