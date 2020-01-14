using System;
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

		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_readyState.SetActive(false);
			_upgradableState.SetActive(false);
			
			_services.MessageBrokerService.Subscribe<MainCurrencyValueChangedEvent>(OnMainCurrencyValueChanged);
		}

		private void Start()
		{
			var info = _dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId);

			UpdateView(info);
			OnReadyToCollect(info.ProductionTime);
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void OnPointerClick(PointerEventData eventData)
		{
			if (_readyState.activeSelf)
			{
				_services.CommandService.ExecuteCommand(new CollectSeedsCommand { Unique = _entityMonoComponent.UniqueId });
				
				_readyState.SetActive(false);
				OnReadyToCollect(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId).ProductionTime);
			}
		}

		public void UpgradeClicked()
		{
			_services.CommandService.ExecuteCommand(new UpgradeBuildingCommand { UniqueId = _entityMonoComponent.UniqueId });
			
			UpdateView(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId));
		}

		private void UpdateView(BuildingInfo info)
		{
			var seedsSec = info.ProductionAmount / info.ProductionTime;

			_buildingNameText.text = $"{info.GameId} {info.Data.Level.ToString()}\n{seedsSec.ToString("0.##")}/s";
			_productionAmountText.text = info.ProductionAmount.ToString();
			_upgradeCostText.text = info.UpgradeCost.ToString();

			UpdateUpgradeState(info.UpgradeCost);
		}

		private void UpdateUpgradeState(float upgradeCost)
		{
			_upgradableState.SetActive(_dataProvider.CurrencyDataProvider.MainCurrencyAmount >= upgradeCost);
		}

		private void OnMainCurrencyValueChanged(MainCurrencyValueChangedEvent eventData)
		{
			UpdateUpgradeState(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.UniqueId).UpgradeCost);
		}

		private async void OnReadyToCollect(float time)
		{
			await Task.Delay(new TimeSpan(0, 0, Mathf.RoundToInt(time)));
			
			_readyState.SetActive(true);
		}
	}
}