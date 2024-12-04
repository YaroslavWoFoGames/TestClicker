using Clicker.Game.Services;
using UnityEngine;

namespace Clicker.Core
{
	// TODO mini version

	public class AudioService : IAudioService
	{
		private readonly AudioConfig _audioConfig;
		private readonly AudioSource _audioSource;

		public AudioService(IGameConfigurationService gameConfigurationService, AudioSource oneShotAudioSource)
		{
			_audioConfig = gameConfigurationService.GameConfiguration.AudioConfig;
			_audioSource = oneShotAudioSource;
		}

		public void PlayOneShotById(AudioOneShotType type)
		{
			_audioSource.clip = _audioConfig.GetAudioClipById(type);
			_audioSource.Play();
		}
	}
}