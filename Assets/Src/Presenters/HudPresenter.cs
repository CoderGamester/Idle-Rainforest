using Events;
using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using Services;
using TMPro;
using UnityEngine;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class HudPresenter : UiPresenter
	{
		[SerializeField] private TextMeshProUGUI _mainCurrencyText;
		[SerializeField] private TextMeshProUGUI _softCurrencyText;
		[SerializeField] private TextMeshProUGUI _hardCurrencyText;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_services.MessageBrokerService.Subscribe<CurrencyValueChangedEvent>(OnSeedsValueChanged);
		}

		private void Start()
		{
			_mainCurrencyText.text = $"MC: {_dataProvider.CurrencyDataProvider.MainCurrencyAmount.ToString()}";
			_softCurrencyText.text = $"SC: {_dataProvider.CurrencyDataProvider.SoftCurrencyAmount.ToString()}";
			_hardCurrencyText.text = $"HC: {_dataProvider.CurrencyDataProvider.HardCurrencyAmount.ToString()}";
		}

		private void OnSeedsValueChanged(CurrencyValueChangedEvent eventData)
		{
			_mainCurrencyText.text =$"MC: {_dataProvider.CurrencyDataProvider.MainCurrencyAmount.ToString()}";
		}
	}
}