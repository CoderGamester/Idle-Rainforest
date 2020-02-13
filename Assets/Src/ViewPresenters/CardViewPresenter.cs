using Commands;
using GameLovers.LoaderExtension;
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
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private TextMeshProUGUI _requirementText;
		[SerializeField] private TextMeshProUGUI _upgradeCostText;
		[SerializeField] private Slider _requirementSlider;
		[SerializeField] private Button _upgradeButton;
		[SerializeField] private Image _image;
		
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

			UpdateView(info);
		}

		private void OnUpgradeClicked()
		{
			_services.CommandService.ExecuteCommand(new UpgradeCardCommand { Card = _card});
		}

		private async void UpdateView(CardInfo info)
		{
			var levelText = info.Data.Level < info.MaxLevel ? info.Data.Level.ToString() : "Max";
			
			_levelText.text = $"Level {levelText}";
			_upgradeCostText.text = $"{info.UpgradeCost.ToString()} HC";
			_requirementText.text = $"{info.Data.Amount.ToString()}/{info.AmountRequired.ToString()}";
			_upgradeButton.interactable = info.Data.Amount >= info.AmountRequired && _dataProvider.CurrencyDataProvider.HardCurrencyAmount >= info.UpgradeCost;
			_requirementSlider.value = (float) info.Data.Amount / info.AmountRequired;
			_image.sprite = await LoaderUtil.LoadAssetAsync<Sprite>($"{AddressablePathLookup.SpritesAnimals}/{info.GameId}.png", false);

			if (info.Data.Amount == 0)
			{
				_requirementSlider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
			}
		}
	}
}