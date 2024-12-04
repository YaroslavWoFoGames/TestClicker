using System;
using Zenject;

namespace Clicker.Core
{
	public class GameStateService : IGameStateService, ITickable, IDisposable
	{
		private readonly GameStateMachine _gameStateMachine;

		public GameStateService(DiContainer container)
		{
			_gameStateMachine = new GameStateMachine(container);
		}

		public void SwitchState<T>(bool shouldRemoveFromCache = false) where T : IGameState
		{
			_gameStateMachine.SwitchState<T>(shouldRemoveFromCache);
		}

		public void SwitchState<TState, TContext>(TContext context, bool shouldRemoveFromCache = false)
			where TState : IGameState<TContext>
			where TContext : IGameStateContext
		{
			_gameStateMachine.SwitchState<TState, TContext>(context, shouldRemoveFromCache);
		}

		public void Tick()
		{
			_gameStateMachine.Tick();
		}

		public void Dispose()
		{
			_gameStateMachine.Dispose();
		}
	}
}