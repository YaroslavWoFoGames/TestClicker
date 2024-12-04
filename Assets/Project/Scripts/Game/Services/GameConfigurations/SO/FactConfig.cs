using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(FactConfig),
		                fileName = nameof(FactConfig))]
	public class FactConfig : Config
	{
		[field: SerializeField] public string ApiKey { get; private set; } = "https://dogapi.dog/docs/api-v2";
	}
}