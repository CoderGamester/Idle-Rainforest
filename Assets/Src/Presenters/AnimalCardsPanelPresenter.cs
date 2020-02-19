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
	public class AnimalCardsPanelPresenter : UiPresenter
	{
		[SerializeField] private Button _closeButton;
		[SerializeField] private AnimalCardViewPresenter _cardRef;
		
		private IGameDataProvider _dataProvider;
		private IObjectPool<AnimalCardViewPresenter> _pool;

		private void Awake()
		{
			const int poolSize = 10;

			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_pool = new GameObjectPool<AnimalCardViewPresenter>(poolSize, _cardRef);
			
			_cardRef.gameObject.SetActive(false);
			_closeButton.onClick.AddListener(Close);
		}

		/// <inheritdoc />
		protected override void OnOpened()
		{
			var cards = _dataProvider.CardDataProvider.GetAnimalCards();
			
			_pool.DespawnAll();
			foreach (var cardInfo in cards)
			{
				_pool.Spawn().SetData(cardInfo);
			}
		}
	}
}