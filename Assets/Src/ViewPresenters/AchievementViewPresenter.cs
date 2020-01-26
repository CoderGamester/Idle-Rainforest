using System;
using Commands;
using Data;
using GameLovers.ConfigsContainer;
using GameLovers.Services;
using Ids;
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
	public class AchievementViewPresenter : MonoBehaviour, IPoolEntitySpawn, IPoolEntityDespawn
	{
		[SerializeField] private TextMeshProUGUI _descriptionText;
		[SerializeField] private Button _collectButton;

		private UniqueId _achievementId = UniqueId.Invalid;
		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		/// <summary>
		/// TODO:
		/// </summary>
		public bool IsActive => _achievementId != UniqueId.Invalid;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_collectButton.onClick.AddListener(OnCompleteClicked);
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void SetData(AchievementData data)
		{
			_achievementId = data.Id;
			
			_dataProvider.AchievementDataProvider.Data.Observe(data.Id, ListUpdateType.Updated, UpdateView);
			UpdateView(data);
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void OnSpawn()
		{
			gameObject.SetActive(true);
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void OnDespawn()
		{
			_dataProvider.AchievementDataProvider.Data.StopObserving(_achievementId, ListUpdateType.Updated, UpdateView);
			gameObject.SetActive(false);
			_achievementId = UniqueId.Invalid;
		}

		private void UpdateView(AchievementData data)
		{
			_descriptionText.text = data.IsCompleted ? "COLLECT" : $"{data.AchievementType}\n{data.CurrentValue.ToString()}/{data.Goal.Value.ToString()}";
			_collectButton.interactable = data.IsCompleted;
		}

		private void OnCompleteClicked()
		{
			_services.CommandService.ExecuteCommand(new CollectAchievementCommand { Achievement = _achievementId });
		}
	}
}