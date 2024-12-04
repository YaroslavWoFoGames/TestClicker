using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(SceneConfig),
		                fileName = nameof(SceneConfig))]
	public class SceneConfig : Config
	{
		[field: SerializeField] public string GameSceneName { get; private set; } = string.Empty;
	}
}