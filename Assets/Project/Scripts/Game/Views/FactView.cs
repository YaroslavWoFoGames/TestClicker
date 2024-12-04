using System;
using Clicker.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Clicker.Game.Views
{
	public class FactViewArgs : IViewArgs
	{
		public readonly string Id;
		public readonly string Number;
		public readonly string Name;
		public readonly IReadOnlyReactiveProperty<bool> IsLoading;
		public readonly Action<string> CallbackClick;

		public FactViewArgs(string id, string number, string name, IReadOnlyReactiveProperty<bool> isLoading,
		                    Action<string> onClick)
		{
			Id = id;
			Number = number;
			Name = name;
			IsLoading = isLoading;
			CallbackClick = onClick;
		}
	}

	public class FactView : ViewWithArgs<FactViewArgs>
	{
		[SerializeField] private TextMeshProUGUI _factNameText;
		[SerializeField] private Button _button;
		[SerializeField] private GameObject _loadingIndicator;

		protected override void OnSetup()
		{
			_factNameText.text = $"{Args.Number} - {Args.Name}";
			Args.IsLoading.Skip(1).Subscribe(HandleChangeLoading).AddTo(CompositeDisposableDeactivate);
			base.OnSetup();
		}

		private void Awake()
		{
			_button.onClick.AddListener(OnButtonClick);
		}

		private void OnDestroy()
		{
			_button.onClick.RemoveListener(OnButtonClick);
		}

		private void OnButtonClick()
		{
			Args.CallbackClick?.Invoke(Args.Id);
		}

		private void HandleChangeLoading(bool isLoading)
		{
			_loadingIndicator.SetActive(isLoading);
		}
	}
}