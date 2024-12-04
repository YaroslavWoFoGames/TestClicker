using Clicker.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Clicker.Game.Services
{
	public sealed class CurrencyService : ICurrencyService, ITickable
	{
		public IReadOnlyReactiveProperty<int> Currency => _currency;

		private readonly IEnergyService _energyService;
		private readonly IAudioService _audioService;
		private readonly ReactiveProperty<int> _currency = new(0);
		private readonly EnergyConfig _energyConfig;
		private readonly CurrencyConfig _currencyConfig;

		private float _time = 0;

		public CurrencyService(IGameConfigurationService gameConfigurationService, IEnergyService energyService,
		                       IAudioService audioService)
		{
			_energyService = energyService;
			_audioService = audioService;
			_energyConfig = gameConfigurationService.GameConfiguration.EnergyConfig;
			_currencyConfig = gameConfigurationService.GameConfiguration.CurrencyConfig;
		}

		public void AddCurrency()
		{
			if (_energyService.TryConsumeEnergy(_energyConfig.CostEnergyPerClick))
			{
				AddValue(_currencyConfig.FixReward);
			}
		}

		void ITickable.Tick()
		{
			_time += Time.deltaTime;
			if (_time >= _currencyConfig.TimeAutoClick)
			{
				if (_energyService.TryConsumeEnergy(_energyConfig.CostEnergyPerAutoClick))
				{
					AddValue(_currencyConfig.FixAutoReward);
					_time = 0;
					Debug.Log("passive income");
				}
			}
		}

		private void AddValue(int value)
		{
			_currency.Value += value;
			_audioService.PlayOneShotById(AudioOneShotType.Tap);
		}
	}
}