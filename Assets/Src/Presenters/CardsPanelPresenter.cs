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
		private IObjectPool<CardViewPresenter> _pool;

		private void Awake()
		{
			const int poolSize = 10;

			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_pool = new ObjectPool<CardViewPresenter>(poolSize, CardInstantiator);
			
			_cardRef.gameObject.SetActive(false);
			_closeButton.onClick.AddListener(Close);
		}

		/// <inheritdoc />
		protected override void OnOpened()
		{
			var cards = _dataProvider.CardDataProvider.GetAllCards();
			
			_pool.DespawnAll();
			foreach (var cardInfo in cards)
			{
				_pool.Spawn().SetData(cardInfo);
			}
		}

		private CardViewPresenter CardInstantiator()
		{
			var newRef = Instantiate(_cardRef, _cardRef.transform.parent);
				
			newRef.gameObject.SetActive(false);
				
			return newRef;
		}
	}
}