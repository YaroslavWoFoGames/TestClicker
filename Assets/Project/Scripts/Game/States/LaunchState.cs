using Clicker.Core;
using Clicker.Game.Services;
using Clicker.Game.Views;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Clicker.Game.States
{
	public class LaunchState : GameState
	{
		[Inject] private ISceneLoadingService _sceneLoadingService;
		[Inject] private IGameConfigurationService _gameConfigurationService;

		protected override async void OnEnter()
		{
			//TODO: симуляция загрузки
			await UniTask.Delay(500);
			var gameScene = _gameConfigurationService.GameConfiguration.SceneConfig.GameSceneName;
			await _sceneLoadingService.LoadSceneAsync(gameScene);

			var args = new SwitchPanelViewArgs(HandleSwitchStateFact, HandleSwitchStateClicker);
			UIService.CreateView<SwitchPanelView, SwitchPanelViewArgs>(args);
			GameStateService.SwitchState<ClickerState>();
		}

		protected override void OnExit() { }

		private void HandleSwitchStateClicker()
		{
			GameStateService.SwitchState<ClickerState>();
		}

		private void HandleSwitchStateFact()
		{
			GameStateService.SwitchState<FactState>();
		}
	}
}