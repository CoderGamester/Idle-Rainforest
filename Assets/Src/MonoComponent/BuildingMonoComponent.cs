using System;
using System.Threading.Tasks;
using Commands;
using GameLovers.Services;
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

		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_readyState.SetActive(false);
		}

		private void Start()
		{
			var info = _dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.Entity);
			var seedsSec = info.ProductionAmount / info.ProductionTime;

			_buildingNameText.text = $"{info.GameId} {info.Level.ToString()}\n{seedsSec.ToString("F")}/s";
			_productionAmountText.text = info.ProductionAmount.ToString();
			
			OnReadyToCollect(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.Entity).ProductionTime);
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void OnPointerClick(PointerEventData eventData)
		{
			if (_readyState.activeSelf)
			{
				_services.CommandService.ExecuteCommand(new CollectSeedsCommand { Entity = _entityMonoComponent.Entity });
				
				_readyState.SetActive(false);
				OnReadyToCollect(_dataProvider.BuildingDataProvider.GetInfo(_entityMonoComponent.Entity).ProductionTime);
			}
		}

		public void UpgradeClicked()
		{
			_services.CommandService.ExecuteCommand(new UpgradeBuildingCommand { Entity = _entityMonoComponent.Entity });
		}

		private async void OnReadyToCollect(float time)
		{
			await Task.Delay(new TimeSpan(0, 0, Mathf.RoundToInt(time)));
			
			_readyState.SetActive(true);
		}
	}
}