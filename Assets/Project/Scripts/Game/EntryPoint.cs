using Clicker.Core;
using Clicker.Game.States;
using Zenject;

namespace Clicker.Game
{
	public class EntryPoint : IInitializable
	{
		private readonly GameStateService _gameStateService;

		public EntryPoint(GameStateService gameStateService)
		{
			_gameStateService = gameStateService;
			Initialize();
		}

		public void Initialize()
		{
			_gameStateService.SwitchState<LaunchState>();
		}
	}
}