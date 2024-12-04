using System;
using System.Collections.Generic;
using System.Linq;
using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	public enum AudioOneShotType
	{
		Tap
	}

	[Serializable]
	public class AudioSetting
	{
		[field: SerializeField] public AudioOneShotType AudioOneShotType { get; private set; }
		[field: SerializeField] public AudioClip AudioClip { get; private set; }
	}

	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(AudioConfig),
		                fileName = nameof(AudioConfig))]
	public class AudioConfig : Config
	{
		[SerializeField] private List<AudioSetting> _audio = new();

		public AudioClip? GetAudioClipById(AudioOneShotType type)
		{
			var audioSetting = _audio.FirstOrDefault(x => x.AudioOneShotType == type);
			if (audioSetting == null)
			{
				Debug.LogError($"[{nameof(AudioConfig)}] Audio {type} not found");
				return null;
			}

			return audioSetting.AudioClip;
		}
	}
}