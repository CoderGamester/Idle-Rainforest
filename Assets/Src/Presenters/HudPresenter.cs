using System;
using Events;
using GameLovers.Services;
using GameLovers.UiService;
using Ids;
using Infos;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewPresenters;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class HudPresenter : UiPresenter
	{
		private const int _achievementCount = 3;
		
		[SerializeField] private TextMeshProUGUI _achievementCountText;
		[SerializeField] private TextMeshProUGUI _countdownText;
		[SerializeField] private TextMeshProUGUI _mainCurrencyText;
		[SerializeField] private TextMeshProUGUI _softCurrencyText;
		[SerializeField] private TextMeshProUGUI _hardCurrencyText;
		[SerializeField] private Slider _achievementProgressBar;
		[SerializeField] private Button _cardsButton;
		[SerializeField] private AchievementViewPresenter _achievementRef;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;
		private IObjectPool<AchievementViewPresenter> _pool;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			_pool = new ObjectPool<AchievementViewPresenter>(3, AchievementInstantiator);
			
			_achievementRef.gameObject.SetActive(false);
			_services.TickService.SubscribeOnUpdate(UpdateCountdown, 1, true);
			_services.MessageBrokerService.Subscribe<MainCurrencyValueChangedEvent>(OnMainCurrencyValueChanged);
			_services.MessageBrokerService.Subscribe<SoftCurrencyValueChangedEvent>(OnSoftCurrencyValueChanged);
			_services.MessageBrokerService.Subscribe<HardCurrencyValueChangedEvent>(OnHardCurrencyValueChanged);
			_services.MessageBrokerService.Subscribe<AchievementCollectedEvent>(OnAchievementCollected);
			_services.MessageBrokerService.Subscribe<RewardGivingEvent>(OnRewardGivingEvent);
			_services.MessageBrokerService.Subscribe<BuildingCollectedEvent>(OnBuildingCollected);
			_cardsButton.onClick.AddListener(OnCardsClicked);
		}

		private void OnBuildingCollected(BuildingCollectedEvent eventData)
		{
			_services.VfxService.PlayUiVfx(_mainCurrencyText.transform.position);
		}

		private void OnRewardGivingEvent(RewardGivingEvent eventData)
		{
			var endPosition = Vector3.zero;

			if (eventData.Reward.GameId == GameId.MainCurrency)
			{
				endPosition = _mainCurrencyText.transform.position;
			}
			else if (eventData.Reward.GameId == GameId.SoftCurrency)
			{
				endPosition = _softCurrencyText.transform.position;
			}
			else if (eventData.Reward.GameId == GameId.HardCurrency)
			{
				endPosition = _hardCurrencyText.transform.position;
			}
			else if(eventData.Reward.GameId.IsInGroup(GameIdGroup.Animal))
			{
				endPosition = _cardsButton.transform.position;
			}
			else
			{
				throw new InvalidOperationException($"Wrong reward type being processed {eventData.Reward.GameId}");
			}
			
			_services.VfxService.PlayUiVfx(endPosition);
		}

		protected override void OnOpened()
		{
			_mainCurrencyText.text = $"MC: {_dataProvider.CurrencyDataProvider.MainCurrencyAmount.ToString()}";
			_softCurrencyText.text = $"SC: {_dataProvider.CurrencyDataProvider.SoftCurrencyAmount.ToString()}";
			_hardCurrencyText.text = $"HC: {_dataProvider.CurrencyDataProvider.HardCurrencyAmount.ToString()}";
			_countdownText.text = (_dataProvider.EventDataProvider.GetEventInfo().EndTime - DateTime.UtcNow).ToString(@"hh\:mm\:ss");
			
			SetAchievementsView();
		}

		private void UpdateCountdown(float deltaTime)
		{
			_countdownText.text = (_dataProvider.EventDataProvider.GetEventInfo().EndTime - DateTime.UtcNow).ToString(@"hh\:mm\:ss");
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

		private void OnAchievementCollected(AchievementCollectedEvent eventData)
		{
			SetAchievementsView();
		}

		private void OnCardsClicked()
		{
			_services.UiService.OpenUi<CardsPanelPresenter>();
		}

		private void SetAchievementsView()
		{
			var achievementInfo = _dataProvider.AchievementDataProvider.GetInfo();
			
			_achievementProgressBar.value = (float) achievementInfo.Collected / achievementInfo.Total;
			_achievementCountText.text = $"{achievementInfo.Collected.ToString()}/{achievementInfo.Total.ToString()}";

			_pool.DespawnAll();
			
			for (int i = 0, j = 0; i < achievementInfo.Total && j <_achievementCount; i++)
			{
				if (achievementInfo.Achievements[i].IsCollected)
				{
					continue;
				}

				_pool.Spawn().SetData(achievementInfo.Achievements[i]);

				j++;
			}
		}

		private AchievementViewPresenter AchievementInstantiator()
		{
			var newRef = Instantiate(_achievementRef, _achievementRef.transform.parent);
				
			newRef.gameObject.SetActive(false);
				
			return newRef;
		}
	}
}