using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using ViewPresenters;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class TreeCardsPanelPresenter : UiPresenter
	{
		[SerializeField] private Button _closeButton;
		[SerializeField] private TreeCardViewPresenter _cardRef;
		
		private IGameDataProvider _dataProvider;
		private IObjectPool<TreeCardViewPresenter> _pool;

		private void Awake()
		{
			const int poolSize = 10;

			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_pool = new GameObjectPool<TreeCardViewPresenter>(poolSize, _cardRef);
			
			_cardRef.gameObject.SetActive(false);
			_closeButton.onClick.AddListener(Close);
		}

		/// <inheritdoc />
		protected override void OnOpened()
		{
			var cards = _dataProvider.CardDataProvider.GetTreeCards();
			
			_pool.DespawnAll();
			foreach (var cardInfo in cards)
			{
				_pool.Spawn().SetData(cardInfo);
			}
		}
	}
}