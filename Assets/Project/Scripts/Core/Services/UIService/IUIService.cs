using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Clicker.Core
{
	public interface IUIService
	{
		TView CreateView<TView>(Transform parent = null) where TView : View;

		IView<TArgs> CreateView<TView, TArgs>(TArgs args, Transform parent = null)
			where TView : View where TArgs : IViewArgs;

		void DeleteView<TView>() where TView : View, IView;

		UniTask ShowView<TView>() where TView : View;
		UniTask HideView<TView>() where TView : View;
	}
}