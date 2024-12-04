using Cysharp.Threading.Tasks;

namespace Clicker.Core
{
	public interface ISceneLoadingService
	{ 
		 UniTask LoadSceneAsync(string sceneName);
	}
}