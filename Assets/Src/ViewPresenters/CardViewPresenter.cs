using Commands;
using GameLovers.Services;
using Ids;
using Infos;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewPresenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CardViewPresenter : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _cardNameText;
		[SerializeField] private TextMeshProUGUI _upgradeCostText;
		[SerializeField] private Button _upgradeButton;
		
		private IGameDataProvider _dataProvider;
		private IGameServices _services;
		private GameId _card;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_upgradeButton.onClick.AddListener(OnUpgradeClicked);
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void SetData(CardInfo info)
		{
			_card = info.GameId;
			_cardNameText.text = $"lvl {info.Data.Level.ToString()} {info.GameId} {info.Data.Amount.ToString()}/{info.AmountRequired.ToString()}";
			_upgradeCostText.text = info.UpgradeCost.ToString();
		}

		private void OnUpgradeClicked()
		{
			var info = _dataProvider.CardDataProvider.GetInfo(_card);

			if (_dataProvider.CurrencyDataProvider.SoftCurrencyAmount >= info.UpgradeCost)
			{
				_services.CommandService.ExecuteCommand(new UpgradeCardCommand { Card = _card});
			}
		}
	}
}