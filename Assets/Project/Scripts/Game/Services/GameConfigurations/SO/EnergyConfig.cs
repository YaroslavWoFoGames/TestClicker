using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(EnergyConfig),
		                fileName = nameof(EnergyConfig))]
	public class EnergyConfig : Config
	{
		[field: SerializeField] public int MaxEnergy { get; private set; } = 1000;
		[field: SerializeField] public int EnergyPerAdditional { get; private set; } = 10;
		[field: SerializeField] public float EnergyPerTimeAdditional { get; private set; } = 10;
		[field: SerializeField] public int CostEnergyPerClick { get; private set; } = 1;
		[field: SerializeField] public int CostEnergyPerAutoClick { get; private set; } = 1;
	}
}