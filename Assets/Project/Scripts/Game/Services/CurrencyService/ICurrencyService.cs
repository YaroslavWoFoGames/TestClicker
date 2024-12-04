using UniRx;

namespace Clicker.Game.Services
{
	public interface ICurrencyService
	{
		void AddCurrency();
		IReadOnlyReactiveProperty<int> Currency { get; }
	}
}