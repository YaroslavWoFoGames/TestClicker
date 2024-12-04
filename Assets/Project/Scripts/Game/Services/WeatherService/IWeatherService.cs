using Clicker.Game.Services;
using UniRx;

namespace Clicker.Core
{
	public interface IWeatherService
	{
		void StopWeatherUpdates();
		void StartWeatherUpdates();
		IReadOnlyReactiveProperty<WeatherData> WeatherInfo { get; }
	}
}