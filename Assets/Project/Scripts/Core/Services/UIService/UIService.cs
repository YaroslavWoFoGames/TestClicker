using System;
using System.Collections.Generic;
using System.Linq;
using Clicker.Game.Services;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;

namespace Clicker.Core.UIService
{
	public class UIService : IUIService
	{
		private readonly IGameConfigurationService _gameConfigurationService;
		private readonly Dictionary<Type, View> _createdPanel = new();
		private readonly Dictionary<Type, View> _createdPopUps = new();
		private readonly Dictionary<Type, View> _createdCommonViews = new();

		private readonly Canvas _rootCanvas;

		public UIService(Canvas rootCanvas,
		                 IGameConfigurationService gameConfigurationService)
		{
			_rootCanvas = rootCanvas;
			_gameConfigurationService = gameConfigurationService;
		}

		public IView<TArgs> CreateView<TView, TArgs>(TArgs args, Transform parent = null)
			where TView : View where TArgs : IViewArgs
		{
			var view = CreateView<TView>(parent);
			if (view is IView<TArgs> viewWithArgs)
			{
				viewWithArgs.Setup(args);
				return viewWithArgs;
			}

			return null;
		}

		public TView CreateView<TView>(Transform parent = null) where TView : View
		{
			var dict = GetDictionaryForType<TView>();

			if (typeof(TView).GetInterface(nameof(IPanel)) != null)
			{
				foreach (var view in _createdPanel)
				{
					if (view.Key == typeof(TView))
					{
						continue;
					}

					view.Value.Hide().Forget();
				}
			}

			if (dict.TryGetValue(typeof(TView), out var existingView))
			{
				existingView.Show().Forget();
				return (TView)existingView;
			}

			var uiConfig = _gameConfigurationService.GameConfiguration.UIConfig;
			var viewPrefab = GetPrefab<TView>(uiConfig);

			if (viewPrefab == null)
			{
				Debug.LogWarning($"Prefab for {typeof(TView).Name} not found in UIConfig.");
				return null;
			}

			var viewInstance = LeanPool.Spawn(viewPrefab, parent == null ? _rootCanvas.transform : parent);
			viewInstance.Show().Forget();
			dict[typeof(TView)] = viewInstance;

			return viewInstance;
		}

		public void DeleteView<TView>() where TView : View, IView
		{
			var dict = GetDictionaryForType<TView>();
			if (dict.TryGetValue(typeof(TView), out var view))
			{
				LeanPool.Despawn(view);
				dict.Remove(typeof(TView));
				Debug.Log($"Deleted view: {typeof(TView).Name}");
			}
			else
			{
				Debug.LogWarning($"View {typeof(TView).Name} not found for deletion.");
			}
		}

		public async UniTask ShowView<TView>() where TView : View
		{
			var dict = GetDictionaryForType<TView>();
			if (dict.TryGetValue(typeof(TView), out var view))
			{
				await view.Show();
			}
			else
			{
				Debug.LogWarning($"View of type {typeof(TView).Name} is not created yet.");
			}
		}

		public async UniTask HideView<TView>() where TView : View
		{
			var dict = GetDictionaryForType<TView>();
			if (dict.TryGetValue(typeof(TView), out var view))
			{
				await view.Hide();
			}
		}

		private TView GetPrefab<TView>(UIConfig uiConfig) where TView : View
		{
			var prefabResolvers = new Dictionary<Type, Func<string, View>>
			{
				{ typeof(IPanel), uiConfig.GetWindowPrefab },
				{ typeof(IPopUp), uiConfig.GetPopupPrefab },
				{ typeof(IView), uiConfig.GetViewPrefab }
			};

			var resolver = prefabResolvers.FirstOrDefault(kvp => typeof(TView).GetInterface(kvp.Key.Name) != null)
			                              .Value;
			// TODO по имени - так у въюхи делать Id смысла не вижу, так как все равно сделал бы через адресаблы.
			return resolver?.Invoke(typeof(TView).Name) as TView;
		}

		private Dictionary<Type, View> GetDictionaryForType<T>()
		{
			if (typeof(T).GetInterface(nameof(IPanel)) != null)
			{
				return _createdPanel;
			}

			if (typeof(T).GetInterface(nameof(IPopUp)) != null)
			{
				return _createdPopUps;
			}

			return _createdCommonViews;
		}
	}
}