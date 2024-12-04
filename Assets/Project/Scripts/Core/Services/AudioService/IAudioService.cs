using Clicker.Game.Services;

namespace Clicker.Core
{
	public interface IAudioService
	{
		void PlayOneShotById(AudioOneShotType type);
	}
}