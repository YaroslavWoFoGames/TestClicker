namespace Clicker.Core
{
	public interface IGameStateService
	{
		public void SwitchState<T>(bool shouldRemoveFromCache = false) where T : IGameState;

		public void SwitchState<TState, TContext>(TContext context, bool shouldRemoveFromCache = false)
			where TState : IGameState<TContext>
			where TContext : IGameStateContext;
	}
}