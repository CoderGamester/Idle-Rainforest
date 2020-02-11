using System.Collections;
using Commands;
using Data;
using Events;
using GameLovers.AddressableIdsScriptGenerator;
using GameLovers.ConfigsContainer;
using GameLovers.LoaderExtension;
using GameLovers.Services;
using Ids;
using Infos;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace MonoComponent
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class BuildingMonoComponent : MonoBehaviour
	{
		[SerializeField] private EntityMonoComponent _entityMonoComponent;
		[SerializeField] private TextMeshProUGUI _buildingNameText;
		[SerializeField] private TextMeshProUGUI _collectValueText;
		[SerializeField] private TextMeshProUGUI _collectionText;
		[SerializeField] private TextMeshProUGUI _upgradeCostText;
		[SerializeField] private TextMeshProUGUI _levelText;
		[SerializeField] private Slider _levelSlider;
		[SerializeField] private Button _collectButton;
		[SerializeField] private Button _automateButton;
		[SerializeField] private Button _upgradeButton;
		[SerializeField] private Image _fillingImage;
		[SerializeField] private SpriteRenderer _image;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;
		private Coroutine _coroutine;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_upgradeButton.onClick.AddListener(OnUpgradeClicked);
			_collectButton.onClick.AddListener(OnCollectClicked);
			_automateButton.onClick.AddListener(OnAutomateClicked);
			_services.MessageBrokerService.Subscribe<MainCurrencyValueChangedEvent>(OnMainCurrencyValueChanged);
			_services.MessageBrokerService.Subscribe<CardUpgradedEvent>(OnCardUpgradedEvent);
		}

		private void OnDestroy()
		{
			_services?.MessageBrokerService?.UnsubscribeAll(this);
			_services?.CoroutineService?.StopCoroutine(_coroutine);
		}

		private async void Start()
		{
			var info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId);

			foreach (var card in info.BuildingCards)
			{
				_dataProvider.CardDataProvider.Data.Observe(card.GameId, ListUpdateType.Added, OnCardAdded);
			}

			UpdateView();
			
			_image.sprite = await LoaderUtil.LoadAssetAsync<Sprite>($"{AddressablePathLookup.SpritesTrees}/{info.GameId}.png", false);
		}

		private void OnUpgradeClicked()
		{
			_services.CommandService.ExecuteCommand(new UpgradeLevelBuildingCommand { BuildingId = _entityMonoComponent.UniqueId });
			
			UpdateView();
		}
		
		private void OnAutomateClicked()
		{
			_services.CommandService.ExecuteCommand(new AutomateBuildingCommand { BuildingId = _entityMonoComponent.UniqueId });

			UpdateView();
		}
		
		private void OnCollectClicked()
		{
			_services.CommandService.ExecuteCommand(new CollectBuildingCommand { BuildingId = _entityMonoComponent.UniqueId });
			
			RestartCircleCoroutine(_dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId));
		}

		private void UpdateView()
		{
			var info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId);

			_buildingNameText.text = $"{info.GameId}";
			_collectValueText.text = info.ProductionAmount.ToString();
			_upgradeCostText.text = info.UpgradeCost.ToString();
			_levelText.text = $"{info.Data.Level.ToString()}/{info.NextBracketLevel.ToString()}";
			_levelSlider.value = info.Data.Level >= info.NextBracketLevel ? 1 : (float) (info.Data.Level % info.BracketSize)/ info.BracketSize;

			UpdateState(info);
			RestartCircleCoroutine(info);
		}

		private void UpdateState(LevelBuildingInfo info)
		{
			_upgradeButton.interactable = _dataProvider.CurrencyDataProvider.MainCurrencyAmount >= info.UpgradeCost;
			_collectButton.interactable = info.AutomationState != AutomationState.Automated;
			_automateButton.gameObject.SetActive(info.AutomationState == AutomationState.Ready);
		}

		private void OnMainCurrencyValueChanged(MainCurrencyValueChangedEvent eventData)
		{
			UpdateState(_dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId));
		}

		private void OnCardUpgradedEvent(CardUpgradedEvent eventData)
		{
			UpdateView();
		}

		private void OnCardAdded(CardData card)
		{
			if (card.Level == 1 && card.Amount == 0)
			{
				UpdateView();
			}
		}

		private void RestartCircleCoroutine(LevelBuildingInfo info)
		{
			if (_coroutine != null)
			{
				_services.CoroutineService.StopCoroutine(_coroutine);
			}
			
			_coroutine = _services.CoroutineService.StartCoroutine(CircleCoroutine(info));
		}

		private IEnumerator CircleCoroutine(LevelBuildingInfo info)
		{
			_collectionText.text = info.AutomationState == AutomationState.Automated ? "Automated" : "";
			_collectButton.interactable = false;

			do
			{
				while (_services.TimeService.DateTimeUtcNow < info.ProductionEndTime)
				{
					yield return null;
				
					var timespan = _services.TimeService.DateTimeUtcNow - info.Data.ProductionStartTime;
					
					_fillingImage.fillAmount =  (float) timespan.TotalSeconds / info.ProductionTime;
				}

				if (info.AutomationState == AutomationState.Automated)
				{
					info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId);
					yield return null;
				}
			} 
			while (info.AutomationState == AutomationState.Automated);

			_fillingImage.fillAmount = 1f;
			_collectionText.text = "Collect";
			_collectButton.interactable = true;
			_coroutine = null;
		}
	}
}