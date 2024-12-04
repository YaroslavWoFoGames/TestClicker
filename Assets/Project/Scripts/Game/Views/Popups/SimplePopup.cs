using System;
using Clicker.Core;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Game.Views
{
	public class SimplePopupArgs : IViewArgs
	{
		public readonly string Title;
		public readonly string Description;
		public readonly Action CallbackClick;

		public SimplePopupArgs(string title, string description, Action callbackClick)
		{
			Title = title;
			Description = description;
			CallbackClick = callbackClick;
		}
	}

	public class SimplePopup : ViewWithArgs<SimplePopupArgs>, IPopUp
	{
		[SerializeField] private GameObject _titleRect = null!;
		[SerializeField] private GameObject _descriptionRect = null!;
		[SerializeField] private GameObject _buttonsRect = null!;

		[SerializeField] private TextMeshProUGUI _title = null!;
		[SerializeField] private TextMeshProUGUI _description = null!;
		[SerializeField] private Button _button = null!;

		protected override void OnSetup()
		{
			_titleRect.SetActive(!string.IsNullOrEmpty(Args.Title));
			_descriptionRect.SetActive(!string.IsNullOrEmpty(Args.Description));
			_title.text = Args.Title;
			_description.text = Args.Description;

			base.OnSetup();
		}

		protected override void OnActivate()
		{
			_button.onClick.AddListener(HandleClick);
			base.OnActivate();
		}

		protected override void OnDeactivate()
		{
			_button.onClick.RemoveListener(HandleClick);
			base.OnDeactivate();
		}

		private void HandleClick()
		{
			Args.CallbackClick?.Invoke();
			Hide().Forget();
		}
	}
}