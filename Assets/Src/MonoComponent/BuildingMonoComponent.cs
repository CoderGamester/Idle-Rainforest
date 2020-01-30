using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Commands;
using Data;
using Events;
using GameLovers.ConfigsContainer;
using GameLovers.Services;
using Infos;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MonoComponent
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class BuildingMonoComponent : MonoBehaviour
	{
		[SerializeField] private EntityMonoComponent _entityMonoComponent;
		[SerializeField] private TextMeshPro _buildingNameText;
		[SerializeField] private TextMeshProUGUI _collectValueText;
		[SerializeField] private TextMeshProUGUI _collectionText;
		[SerializeField] private TextMeshPro _upgradeCostText;
		[SerializeField] private Button _collectButton;
		[SerializeField] private Button _automateButton;
		[SerializeField] private Image _fillingImage;
		[SerializeField] private GameObject _upgradableState;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;
		private Coroutine _coroutine;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_upgradableState.SetActive(false);
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

		private void Start()
		{
			var info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId);

			foreach (var card in info.BuildingCards)
			{
				_dataProvider.CardDataProvider.Data.Observe(card.GameId, ListUpdateType.Added, OnCardAdded);
			}

			UpdateView();
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void UpgradeClicked()
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
			
			StartCircleCoroutine(_dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId));
		}

		private void UpdateView()
		{
			var info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId);
			var seedsSec = info.ProductionAmount / info.ProductionTime;

			_buildingNameText.text = $"{info.GameId} - lv {info.Data.Level.ToString()}/{info.NextBracketLevel.ToString()}\n" +
			                         $"{seedsSec.ToString("0.##")}/s";
			_collectValueText.text = info.ProductionAmount.ToString();
			_upgradeCostText.text = info.UpgradeCost.ToString();

			UpdateState(info);
			StartCircleCoroutine(info);
		}

		private void UpdateState(LevelBuildingInfo info)
		{
			_upgradableState.SetActive(_dataProvider.CurrencyDataProvider.MainCurrencyAmount >= info.UpgradeCost);
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
			if (card.Level > 1)
			{
				return;
			}
			
			UpdateView();
		}

		private void StartCircleCoroutine(LevelBuildingInfo info)
		{
			if (_coroutine != null)
			{
				_services.CoroutineService.StopCoroutine(_coroutine);
			}
			
			_coroutine = _services.CoroutineService.StartCoroutine(CircleCoroutine(info));
		}

		private IEnumerator CircleCoroutine(LevelBuildingInfo info)
		{
			var loop = true;
			
			_collectionText.text = info.AutomationState == AutomationState.Automated ? "Automated" : "";
			_collectButton.interactable = false;

			while (loop)
			{
				var timespan = _services.TimeService.DateTimeUtcNow - info.Data.ProductionStartTime;

				while (timespan.TotalSeconds < info.ProductionTime)
				{
					yield return null;
				
					_fillingImage.fillAmount =  (float) timespan.TotalSeconds / info.ProductionTime;
					timespan = _services.TimeService.DateTimeUtcNow - info.Data.ProductionStartTime;
				}

				loop = info.AutomationState == AutomationState.Automated;
				
				if (info.AutomationState == AutomationState.Automated)
				{
					info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_entityMonoComponent.UniqueId);
				}
			}

			_fillingImage.fillAmount = 1f;
			_collectionText.text = "Collect";
			_collectButton.interactable = true;
			_coroutine = null;
		}
	}
}