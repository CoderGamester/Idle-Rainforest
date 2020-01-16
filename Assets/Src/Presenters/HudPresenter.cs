using System;
using Events;
using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class HudPresenter : UiPresenter
	{
		[SerializeField] private TextMeshProUGUI _countdownText;
		[SerializeField] private TextMeshProUGUI _mainCurrencyText;
		[SerializeField] private TextMeshProUGUI _softCurrencyText;
		[SerializeField] private TextMeshProUGUI _hardCurrencyText;
		[SerializeField] private Button _cardsButton;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_services.TickService.SubscribeOnUpdate(UpdateCountdown, 1, true);
			_services.MessageBrokerService.Subscribe<MainCurrencyValueChangedEvent>(OnMainCurrencyValueChanged);
			_services.MessageBrokerService.Subscribe<SoftCurrencyValueChangedEvent>(OnSoftCurrencyValueChanged);
			_services.MessageBrokerService.Subscribe<HardCurrencyValueChangedEvent>(OnHardCurrencyValueChanged);
			_cardsButton.onClick.AddListener(OnCardsClicked);
		}

		private void Start()
		{
			_mainCurrencyText.text = $"MC: {_dataProvider.CurrencyDataProvider.MainCurrencyAmount.ToString()}";
			_softCurrencyText.text = $"SC: {_dataProvider.CurrencyDataProvider.SoftCurrencyAmount.ToString()}";
			_hardCurrencyText.text = $"HC: {_dataProvider.CurrencyDataProvider.HardCurrencyAmount.ToString()}";
		}

		private void UpdateCountdown(float deltaTime)
		{
			_countdownText.text = (_dataProvider.BuildingDataProvider.GetEventInfo().EndTime - DateTime.UtcNow).ToString(@"hh\:mm\:ss");
		}

		private void OnMainCurrencyValueChanged(MainCurrencyValueChangedEvent eventData)
		{
			_mainCurrencyText.text =$"MC: {eventData.NewValue.ToString()}";
		}

		private void OnHardCurrencyValueChanged(HardCurrencyValueChangedEvent eventData)
		{
			_hardCurrencyText.text = $"HC: {eventData.NewValue.ToString()}";
		}

		private void OnSoftCurrencyValueChanged(SoftCurrencyValueChangedEvent eventData)
		{
			_softCurrencyText.text = $"SC: {eventData.NewValue.ToString()}";
		}

		private void OnCardsClicked()
		{
			_services.UiService.OpenUi<CardsPanelPresenter>();
		}
	}
}