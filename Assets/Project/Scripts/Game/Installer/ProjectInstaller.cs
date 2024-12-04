using System.Linq;
using Clicker.Core;
using Clicker.Core.RequestQueue;
using Clicker.Core.UIService;
using Clicker.Game.Services;
using Clicker.Game.States;
using UnityEngine;
using Zenject;

namespace Clicker.Game.Bootstrap
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private GameConfiguration _gameConfiguration = null!;
		[SerializeField] private Canvas _rootCanvas = null;
		[SerializeField] private AudioSource _audioSource = null;

		public override void InstallBindings()
		{
			Container.BindInstance(_gameConfiguration).AsSingle();
			Container.BindInstance(_rootCanvas).AsSingle();
			Container.BindInstance(_audioSource).AsSingle();

			Container.BindInterfacesAndSelfTo<UIService>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameConfigurationService>().AsSingle();
			Container.BindInterfacesAndSelfTo<SceneLoadingService>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameStateService>().AsCached();
			Container.BindInterfacesAndSelfTo<AudioService>().AsSingle();

			Container.BindInterfacesAndSelfTo<RequestQueue>().AsSingle();

			Container.BindInterfacesAndSelfTo<WeatherService>().AsSingle();
			Container.BindInterfacesAndSelfTo<FactService>().AsSingle();
			Container.BindInterfacesAndSelfTo<EnergyService>().AsSingle();
			Container.BindInterfacesAndSelfTo<CurrencyService>().AsSingle();
			RegisterAllGameStates();
		}

		private void RegisterAllGameStates()
		{
			var gameStateTypes = typeof(IGameState).Assembly
			                                       .GetTypes()
			                                       .Where(t => typeof(IGameState).IsAssignableFrom(t) && t.IsClass &&
				                                              !t.IsAbstract);

			foreach (var stateType in gameStateTypes)
			{
				Container.Bind(stateType).AsTransient();
			}
		}
	}
}