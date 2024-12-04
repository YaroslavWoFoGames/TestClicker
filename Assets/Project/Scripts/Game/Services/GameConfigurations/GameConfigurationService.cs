namespace Clicker.Game.Services
{
	public class GameConfigurationService : IGameConfigurationService
	{
		public GameConfiguration GameConfiguration { get; }

		public GameConfigurationService(GameConfiguration gameConfiguration)
		{
			GameConfiguration = gameConfiguration;
		}
	}
}