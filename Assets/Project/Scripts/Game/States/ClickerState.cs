using Clicker.Core;
using Clicker.Core.RequestQueue;
using Clicker.Game.Services;
using Clicker.Game.Views;
using Zenject;

namespace Clicker.Game.States
{
	public class ClickerState : GameState
	{
		[Inject] private IEnergyService _energyService;
		[Inject] private ICurrencyService _currencyService;
		[Inject] private IWeatherService _weatherService;

		protected override void OnEnter()
		{
			_weatherService.StartWeatherUpdates();
			var args = new ClickerPanelArgs(_currencyService.AddCurrency, _currencyService.Currency,
			                                _energyService.Energy, _weatherService.WeatherInfo);
			UIService.CreateView<ClickerPanel, ClickerPanelArgs>(args);
		}

		protected override void OnExit()
		{
			_weatherService.StopWeatherUpdates();
			UIService.HideView<ClickerPanel>();
		}
	}
}