using Zenject;

namespace Clicker.Game.Bootstrap
{
	public class LaunchInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<EntryPoint>().AsSingle().NonLazy();
		}
	}
}