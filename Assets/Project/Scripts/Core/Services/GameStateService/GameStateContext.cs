namespace Clicker.Core
{
	public interface IGameStateContext
	{
		IGameState? PreviousGameState { get; }
		bool IsPreviousState { get; }
	}

	public class GameStateContext : IGameStateContext
	{
		public IGameState? PreviousGameState { get; }
		public bool IsPreviousState => PreviousGameState != null;

		public GameStateContext(IGameState? previousGameState)
		{
			PreviousGameState = previousGameState;
		}
	}
}