using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clicker.Core
{
	public class SceneLoadingService : ISceneLoadingService
	{
		public async UniTask LoadSceneAsync(string sceneName)
		{
			await SceneManager.LoadSceneAsync(sceneName);
		}
	}
}