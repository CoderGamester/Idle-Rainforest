using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Commands;
using Events;
using GameLovers.Services;
using Infos;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MonoComponent
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class BuildingMonoComponent : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private EntityMonoComponent _entityMonoComponent;
		[SerializeField] private TextMeshPro _buildingNameText;
		[SerializeField] private TextMeshPro _productionAmountText;
		[SerializeField] private TextMeshPro _upgradeCostText;
		[SerializeField] private GameObject _readyState;
		[SerializeField] private GameObject _upgradableState;
		[SerializeField] private GameObject _automateState;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;
		private CancellationTokenSource _cancellationToken;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_readyState.SetActive(false);
			_upgradableState.SetActive(false);
			
			_services.MessageBrokerService.Subscribe<MainCurrencyValueChangedEvent>(OnMainCurrencyValueChanged);
		}

		private void OnDestroy()
		{
			_cancellationToken?.Cancel();
		}

		private void Start()
		{
			var info = _dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId);

			UpdateView(info);

			if (info.AutomationState != AutomationState.Automated)
			{
				OnReadyToCollect(info.ProductionTime);
			}
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void OnPointerClick(PointerEventData eventData)
		{
			if (_readyState.activeSelf)
			{
				_services.CommandService.ExecuteCommand(new CollectCommand { BuildingId = _entityMonoComponent.UniqueId });
				_readyState.SetActive(false);
				
				OnReadyToCollect(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId).ProductionTime);
			}
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void UpgradeClicked()
		{
			_services.CommandService.ExecuteCommand(new UpgradeLevelBuildingCommand { BuildingId = _entityMonoComponent.UniqueId });
			
			UpdateView(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId));
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void AutomateClicked()
		{
			_services.CommandService.ExecuteCommand(new AutomateBuildingCommand { BuildingId = _entityMonoComponent.UniqueId });
			
			_automateState.SetActive(false);
		}

		private void UpdateView(BuildingInfo info)
		{
			var seedsSec = info.ProductionAmount / info.ProductionTime;

			_buildingNameText.text = $"{info.GameId} {info.Data.Level.ToString()}\n{seedsSec.ToString("0.##")}/s";
			_productionAmountText.text = info.ProductionAmount.ToString();
			_upgradeCostText.text = info.UpgradeCost.ToString();

			UpdateState(info);
		}

		private void UpdateState(BuildingInfo info)
		{
			_upgradableState.SetActive(_dataProvider.CurrencyDataProvider.MainCurrencyAmount >= info.UpgradeCost);
			_automateState.SetActive(info.AutomationState == AutomationState.Ready);
		}

		private void OnMainCurrencyValueChanged(MainCurrencyValueChangedEvent eventData)
		{
			UpdateState(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId));
		}

		private async void OnReadyToCollect(float time)
		{
			_cancellationToken = new CancellationTokenSource();

			try
			{
				await Task.Delay(new TimeSpan(0, 0, Mathf.RoundToInt(time)), _cancellationToken.Token);
			}
			catch (Exception)
			{
				// ignored
			}

			_readyState.SetActive(true);
			_cancellationToken.Dispose();
		}
	}
}