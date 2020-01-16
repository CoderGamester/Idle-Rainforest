using GameLovers.Services;
using GameLovers.UiService;
using Ids;
using Logic;
using Services;
using UnityEngine;
using UnityEngine.UI;
using ViewPresenters;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CardsPanelPresenter : UiPresenter
	{
		[SerializeField] private Button _closeButton;
		[SerializeField] private CardViewPresenter _cardRef;
		
		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			const int poolSize = 10;

			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_cardRef.gameObject.SetActive(false);
			_closeButton.onClick.AddListener(OnCloseClicked);
			_services.PoolService.InitPool(poolSize, _cardRef, card =>
			{
				var newRef = Instantiate(card, _cardRef.transform.parent);
				
				newRef.gameObject.SetActive(false);
				
				return newRef;
			});
		}

		protected override void OnOpened()
		{
			var cards = _dataProvider.CardDataProvider.GetAllCards();
			
			_services.PoolService.DespawnAll<CardViewPresenter>();
			foreach (var cardInfo in cards)
			{
				var card = _services.PoolService.Spawn<CardViewPresenter>();
				
				card.gameObject.SetActive(true);
				card.SetData(cardInfo);
			}
		}

		private void OnCloseClicked()
		{
			_services.UiService.CloseUi<CardsPanelPresenter>();
		}
	}
}