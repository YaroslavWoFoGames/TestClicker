using System;
using System.Collections.Generic;
using Zenject;

namespace Clicker.Core
{
	public class GameStateMachine : ITickable, IDisposable
	{
		public IGameState CurrentState => _currentState!;

		private readonly Dictionary<Type, IGameState> _stateCache = new();
		private readonly DiContainer _container;
		private IGameState? _currentState;

		public GameStateMachine(DiContainer container)
		{
			_container = container;
		}

		public void SwitchState<T>(bool shouldRemoveFromCache) where T : IGameState
		{
			var state = CreateState<T>(shouldRemoveFromCache);
			ChangeState(state);
		}

		public void SwitchState<TState, TContext>(TContext context, bool shouldRemoveFromCache)
			where TState : IGameState<TContext>
			where TContext : IGameStateContext
		{
			var state = CreateState<TState, TContext>(context, shouldRemoveFromCache);
			ChangeState(state);
		}

		public void Dispose()
		{
			_currentState?.Dispose();
			_currentState?.Exit();
		}

		private void ChangeState(IGameState? gameState)
		{
			if (_currentState != null)
			{
				_currentState.Exit();

				if (_stateCache.TryGetValue(typeof(IGameState), out var cachedState) && cachedState == _currentState &&
				    cachedState.ShouldRemoveFromCache)
				{
					_stateCache.Remove(typeof(IGameState));
					_currentState.Dispose();
				}
			}

			_currentState = gameState;

			if (_currentState != null)
			{
				_currentState.SetStateMachine(this);
				_currentState.Enter();
			}
		}

		private T CreateState<T>(bool shouldRemoveFromCache) where T : IGameState
		{
			var key = typeof(T);

			if (!_stateCache.TryGetValue(key, out var cachedState))
			{
				var newState = _container.Resolve<T>();
				_stateCache[key] = newState;
				newState.ShouldRemoveFromCache = shouldRemoveFromCache;
				return newState;
			}

			return (T)cachedState;
		}

		private IGameState CreateState<TState, TContext>(TContext context, bool shouldRemoveFromCache)
			where TState : IGameState<TContext>
			where TContext : IGameStateContext
		{
			var state = CreateState<TState>(shouldRemoveFromCache);
			state.SetContext(context);
			return state;
		}

		public void Tick()
		{
			_currentState?.Tick();
		}
	}
}