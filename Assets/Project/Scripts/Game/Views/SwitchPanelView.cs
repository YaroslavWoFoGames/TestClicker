using System;
using Clicker.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Game.Views
{
	public class SwitchPanelViewArgs : IViewArgs
	{
		public readonly Action OnFactsSelected;
		public readonly Action OnClickerSelected;

		public SwitchPanelViewArgs(Action onFactsSelected, Action onClickerSelected)
		{
			OnFactsSelected = onFactsSelected;
			OnClickerSelected = onClickerSelected;
		}
	}

	public class SwitchPanelView : ViewWithArgs<SwitchPanelViewArgs>
	{
		[SerializeField] private Button _buttonFacts = null!;
		[SerializeField] private Button _buttonClicker = null!;

		protected override void OnActivate()
		{
			base.OnActivate();
			_buttonFacts.onClick.AddListener(HandleFactsButtonClick);
			_buttonClicker.onClick.AddListener(HandleClickerButtonClick);
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_buttonFacts.onClick.RemoveListener(HandleFactsButtonClick);
			_buttonClicker.onClick.RemoveListener(HandleClickerButtonClick);
		}

		private void HandleFactsButtonClick()
		{
			Args?.OnFactsSelected?.Invoke();
			UpdateButtonStates(true);
		}

		private void HandleClickerButtonClick()
		{
			Args?.OnClickerSelected?.Invoke();
			UpdateButtonStates(false);
		}

		private void UpdateButtonStates(bool isFactsSelected = false)
		{
			_buttonFacts.interactable = !isFactsSelected;
			_buttonClicker.interactable = isFactsSelected;
		}
	}
}