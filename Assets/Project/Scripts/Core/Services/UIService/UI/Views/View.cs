using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UniRx;
using UnityEngine;

namespace Clicker.Core
{
	public interface IViewArgs { }
	
	public abstract class ViewWithArgs<TArgs> : View, IView<TArgs> where TArgs : IViewArgs
	{
		public TArgs Args { get; private set; }

		public virtual void Setup(TArgs args)
		{
			Args = args;
			OnSetup();
		}

		protected virtual void OnSetup() { }
	}
	
	public abstract class View : MonoBehaviour
	{
		[field: SerializeField] public RectTransform RectTransform { get; private set; }
		[field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

		protected CompositeDisposable CompositeDisposableDeinitialize = new();
		protected CompositeDisposable CompositeDisposableDeactivate = new();

		private CancellationTokenSource _currentShowCancellationTokenSource;
		private CancellationTokenSource _currentHideCancellationTokenSource;

		private bool _isAnimatingShow;
		private bool _isAnimatingHide;

		private List<IView> _dynamicViews = new();

		public void StretchToFitParent()
		{
			RectTransform.anchorMin = Vector2.zero;
			RectTransform.anchorMax = Vector2.one;
			RectTransform.anchoredPosition = Vector2.zero;
			RectTransform.sizeDelta = Vector2.zero;
		}

		public virtual UniTask AnimateShow()
		{
			return UniTask.CompletedTask;
		}

		public virtual UniTask AnimateHide()
		{
			return UniTask.CompletedTask;
		}

		public virtual async UniTask Show()
		{
			gameObject.SetActive(true);

			_currentHideCancellationTokenSource?.Cancel();
			_currentShowCancellationTokenSource = new CancellationTokenSource();
			await AnimateShow().AttachExternalCancellation(_currentShowCancellationTokenSource.Token);
		}

		public virtual async UniTask Hide()
		{
			_currentShowCancellationTokenSource?.Cancel();
			_currentHideCancellationTokenSource = new CancellationTokenSource();
			await AnimateHide().AttachExternalCancellation(_currentHideCancellationTokenSource.Token);

			gameObject.SetActive(false);
		}

		protected IView<TArgs> CreateDynamicView<TArgs>(TArgs args, View view, RectTransform content)
			where TArgs : IViewArgs
		{
			var viewInstance = LeanPool.Spawn(view, content) as IView<TArgs>;
			if (viewInstance != null)
			{
				viewInstance.Setup(args);
				_dynamicViews.Add(viewInstance);
			}
			else
			{
				Debug.LogError("view not casted IView<TArgs>");
			}

			return viewInstance;
		}

		protected void DeleteDynamicView(IView view)
		{
			if (_dynamicViews.Contains(view))
			{
				LeanPool.Despawn(view as View);
				_dynamicViews.Remove(view);
			}
		}

		protected void DeleteAllDynamicViews()
		{
			foreach (var dynamicView in _dynamicViews)
			{
				LeanPool.Despawn(dynamicView as View);
			}
			_dynamicViews.Clear();
		}

		protected virtual void OnActivate() { }

		protected virtual void OnDeactivate() { }

		protected virtual void OnInitialize() { }

		protected virtual void OnDeinitialize() { }

		protected virtual void OnTick() { }

		private void Awake()
		{
			OnInitialize();
		}

		private void OnDestroy()
		{
			OnDeinitialize();
			CompositeDisposableDeinitialize.Dispose();
		}

		private void OnEnable()
		{
			OnActivate();
		}

		private void OnDisable()
		{
			OnDeactivate();
		}

		private void Tick()
		{
			OnTick();
		}

#if UNITY_EDITOR
		private void OnValidate()
		{
			if (RectTransform == null)
			{
				RectTransform = GetComponent<RectTransform>();
			}
		}
#endif
	}
}