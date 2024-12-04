using UniRx;
using UnityEngine;
using Zenject;

namespace Clicker.Game.Services
{
	public class EnergyService : IEnergyService, ITickable
	{
		public IReadOnlyReactiveProperty<int> Energy => _energy;
		private readonly ReactiveProperty<int> _energy;
		private readonly EnergyConfig _energyConfig;

		private float _time;

		public EnergyService(IGameConfigurationService gameConfigurationService)
		{
			_energyConfig = gameConfigurationService.GameConfiguration.EnergyConfig;
			_energy = new ReactiveProperty<int>(_energyConfig.MaxEnergy);
		}

		public void AddEnergy(int amount)
		{
			if (amount < 0)
			{
				Debug.LogError("energy negative amount not added");
				return;
			}

			_energy.Value = Mathf.Min(_energy.Value + amount, _energyConfig.MaxEnergy);
		}

		public bool TryConsumeEnergy(int amount)
		{
			if (amount < 0)
			{
				Debug.LogError("energy negative amount not consume");
				return false;
			}

			if (_energy.Value >= amount)
			{
				_energy.Value -= amount;
				return true;
			}

			return false;
		}

		void ITickable.Tick()
		{
			_time += Time.deltaTime;
			if (_time >= _energyConfig.EnergyPerTimeAdditional)
			{
				AddEnergy(_energyConfig.EnergyPerAdditional);
				_time = 0;
				Debug.Log("energy - passive added");
			}
		}
	}
}