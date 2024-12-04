using System;
using System.Collections.Generic;
using System.Linq;
using Clicker.Core;
using Clicker.Core.SO;
using UnityEngine;

namespace Clicker.Game.Services
{
	[Serializable]
	public class ViewConfig
	{
		[field: SerializeField] public string Id { get; private set; }
		[field: SerializeField] public View Prefab { get; private set; }
	}

	// TODO: тут я сделал так, но вообще нужно через адресаблы и ресурс сервис
	[CreateAssetMenu(menuName = AssetMenuPaths.GameConfigurations + nameof(UIConfig), fileName = nameof(UIConfig))]
	public class UIConfig : Config
	{
		[SerializeField] private List<ViewConfig> _panels = new();
		[SerializeField] private List<ViewConfig> _views = new();
		[SerializeField] private List<ViewConfig> _popups = new();

		public View? GetWindowPrefab(string id)
		{
			var viewConfig = _panels.FirstOrDefault(x => x.Id == id);
			if (viewConfig == null)
			{
				Debug.LogError($"[{nameof(UIConfig)}] Prefab not found by id:{id}");
				return null;
			}

			return viewConfig.Prefab;
		}

		public View? GetViewPrefab(string id)
		{
			var viewConfig = _views.FirstOrDefault(x => x.Id == id);

			if (viewConfig == null)
			{
				Debug.LogError($"[{nameof(UIConfig)}] Prefab not found by id:{id}");
				return null;
			}

			return viewConfig.Prefab;
		}

		public View? GetPopupPrefab(string id)
		{
			var viewConfig = _popups.FirstOrDefault(x => x.Id == id);
			if (viewConfig == null)
			{
				Debug.LogError($"[{nameof(UIConfig)}] Prefab not found by id:{id}");
				return null;
			}

			return viewConfig.Prefab;
		}
	}
}