using UniRx;

namespace Clicker.Game.Services
{
	public interface IEnergyService
	{
		void AddEnergy(int amount);
		bool TryConsumeEnergy(int amount);
		IReadOnlyReactiveProperty<int> Energy { get; }
	}
}