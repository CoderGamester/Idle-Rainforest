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
	public class AchievementViewPresenter : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _descriptionText;
		[SerializeField] private Button _collectButton;

		private UniqueId _achievementId;
		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_collectButton.onClick.AddListener(OnCompleteClicked);
		}

		private void OnDisable()
		{
			_dataProvider?.AchievementDataProvider?.Data?.StopObserving(_achievementId, ListUpdateType.Updated, UpdateView);
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