using System;
using System.Collections.Generic;
using Clicker.Core;
using Clicker.Game.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Clicker.Game.Views
{
	public class FactPanelArgs : IViewArgs
	{
		public readonly IReadOnlyReactiveProperty<List<DogFact>> DogFacts;
		public readonly Action<string> CallbackClick;

		public FactPanelArgs(IReadOnlyReactiveProperty<List<DogFact>> dogFacts, Action<string> callbackClick)
		{
			DogFacts = dogFacts;
			CallbackClick = callbackClick;
		}
	}

	public class FactPanel : ViewWithArgs<FactPanelArgs>, IPanel
	{
		[SerializeField] private RectTransform _factsContainer;
		[SerializeField] private View _factViewPrefab;
		[SerializeField] private GameObject _loadingView;

		public override UniTask AnimateHide()
		{
			return CanvasGroup.DOFade(0f, 0.5f)
			                  .SetEase(Ease.InOutQuad)
			                  .AsyncWaitForCompletion().AsUniTask();
		}

		public override UniTask AnimateShow()
		{
			return CanvasGroup.DOFade(1f, 0.5f)
			                  .SetEase(Ease.InOutQuad)
			                  .AsyncWaitForCompletion().AsUniTask();
		}

		public override void Setup(FactPanelArgs args)
		{
			base.Setup(args);

			Args.DogFacts.Skip(1).Subscribe(UpdateFacts).AddTo(CompositeDisposableDeactivate);
			_loadingView.SetActive(true);
		}

		protected override void OnDeactivate()
		{
			DeleteAllDynamicViews();
			base.OnDeactivate();
		}

		private void UpdateFacts(List<DogFact> facts)
		{
			DeleteAllDynamicViews();

			for (var i = 0; i < facts.Count; i++)
			{
				var args = new FactViewArgs(facts[i].id, (i + 1).ToString(), facts[i].name, facts[i].IsLoading,
				                            Args.CallbackClick);
				CreateDynamicView(args, _factViewPrefab, _factsContainer);
			}

			_loadingView.SetActive(false);
		}
	}
}