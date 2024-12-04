using System;
using Clicker.Core;
using Clicker.Game.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace Clicker.Game.Views
{
	public class ClickerPanelArgs : IViewArgs
	{
		public readonly Action CallbackClick;
		public readonly IReadOnlyReactiveProperty<int> CurrencyValue;
		public readonly IReadOnlyReactiveProperty<int> EnergyValue;
		public readonly IReadOnlyReactiveProperty<WeatherData> WeatherInfo;

		public ClickerPanelArgs(Action callbackClick, IReadOnlyReactiveProperty<int> currencyValue,
		                        IReadOnlyReactiveProperty<int> energyValue,
		                        IReadOnlyReactiveProperty<WeatherData> weatherInfo)
		{
			CallbackClick = callbackClick;
			CurrencyValue = currencyValue;
			EnergyValue = energyValue;
			WeatherInfo = weatherInfo;
		}
	}

	public class ClickerPanel : ViewWithArgs<ClickerPanelArgs>, IPanel
	{
		[SerializeField] private TextMeshProUGUI _currency = null!;
		[SerializeField] private TextMeshProUGUI _energy = null!;
		[SerializeField] private TextMeshProUGUI _weatherText = null!;
		[SerializeField] private TextMeshProUGUI _addedCurrencyValue = null!;

		[SerializeField] private Image _weatherIcon = null!;
		[SerializeField] private Button _clickButton = null!;
		[SerializeField] private ParticleSystem _particleSystem = null!;

		private Tween _tween;
		private Sequence _sequenceText;
		private WeatherData _lastWeatherData;

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

		protected override void OnSetup()
		{
			CompositeDisposableDeactivate.Clear();
			Args.CurrencyValue.Skip(1).Subscribe(HandleChangedCurrency).AddTo(CompositeDisposableDeactivate);
			Args.EnergyValue.Subscribe(HandleChangedEnergy).AddTo(CompositeDisposableDeactivate);
			Args.WeatherInfo.Skip(1).Subscribe(HandleChangedWeather).AddTo(CompositeDisposableDeactivate);

			base.OnSetup();
		}

		protected override void OnActivate()
		{
			_tween?.Kill();
			_sequenceText?.Kill();
			_clickButton.onClick.AddListener(HandleButtonClick);
			base.OnActivate();
		}

		protected override void OnDeactivate()
		{
			_tween?.Kill();
			_sequenceText?.Kill();
			_clickButton.onClick.RemoveListener(HandleButtonClick);
			base.OnDeactivate();
		}

		private void HandleButtonClick()
		{
			Args.CallbackClick?.Invoke();
			PlayEffect();
		}

		private void PlayEffect()
		{
			_particleSystem.Play();
			AnimateButtonClick();
			_addedCurrencyValue.rectTransform.anchoredPosition = new Vector2(0, 64);
			_addedCurrencyValue.rectTransform.localScale = Vector3.one;
			_sequenceText?.Kill();
			_sequenceText = DOTween.Sequence();
			_sequenceText.Append(_addedCurrencyValue.rectTransform.DOAnchorPosY(133, 1).SetEase(Ease.OutQuad))
			             .Join(_addedCurrencyValue.rectTransform.DOScale(Vector3.zero, 1).SetEase(Ease.OutQuad));
		}

		private void HandleChangedCurrency(int value = 1)
		{
			PlayEffect();
			_currency.text = $"Currency: {value}";
			_addedCurrencyValue.text = "+1"; // TODO
		}

		private void HandleChangedEnergy(int value = 1)
		{
			_energy.text = $"Energy: {value}";
		}

		private void HandleChangedWeather(WeatherData weatherData)
		{
			if (_lastWeatherData?.WeatherResponse?.properties.periods?[0]?.name ==
			    weatherData?.WeatherResponse?.properties.periods?[0]?.name)
			{
				return;
			}

			_lastWeatherData = weatherData;

			if (weatherData != null)
			{
				if (weatherData.WeatherResponse != null && weatherData.WeatherResponse.properties.periods != null)
				{
					var today = weatherData.WeatherResponse.properties.periods[0];
					_weatherIcon.sprite = weatherData.Sprite;
					_weatherText.text = $"{today.shortForecast}: {today.temperature}{today.temperatureUnit}";
				}
			}
		}

		private void AnimateButtonClick()
		{
			_tween?.Kill();
			_tween = _clickButton.transform.DOScale(0.9f, 0.1f)
			                     .SetEase(Ease.OutQuad)
			                     .OnComplete(() =>
			                     {
				                     _clickButton.transform.DOScale(1f, 0.1f)
				                                 .SetEase(Ease.OutBounce);
			                     });
		}
	}
}