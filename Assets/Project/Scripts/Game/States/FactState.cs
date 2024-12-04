using Clicker.Core;
using Clicker.Game.Services;
using Clicker.Game.Views;
using Zenject;

namespace Clicker.Game.States
{
	public class FactState : GameState
	{
		[Inject] private IFactService _factService;

		protected override void OnEnter()
		{
			_factService.CancelDogFactsRequest();
			_factService.CancelDogFactDetailsRequest();
			_factService.FetchDogFacts();
			var args = new FactPanelArgs(_factService.DogFacts, HandleRequestFactDetail);
			UIService.CreateView<FactPanel, FactPanelArgs>(args);
		}

		protected override void OnExit()
		{
			_factService.CancelDogFactsRequest();
			_factService.CancelDogFactDetailsRequest();
			UIService.HideView<FactPanel>();
		}

		private void HandleRequestFactDetail(string id)
		{
			_factService.FetchDogFactDetails(id, HandleShowPopup);
		}

		private void HandleShowPopup(DogFactDetails details)
		{
			var args = new SimplePopupArgs(
			                               details.name,
			                               details.description,
			                               null
			                              );

			UIService.CreateView<SimplePopup, SimplePopupArgs>(args);
		}
	}
}