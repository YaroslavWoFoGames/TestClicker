using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(CurrencyConfig),
		                fileName = nameof(CurrencyConfig))]
	public class CurrencyConfig : Config
	{
		[field: SerializeField] public float TimeAutoClick { get; private set; } = 3;
		[field: SerializeField] public int FixReward { get; private set; } = 1;
		[field: SerializeField] public int FixAutoReward { get; private set; } = 1;
	}
}