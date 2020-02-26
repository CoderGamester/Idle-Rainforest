using System;
using Commands;
using Data;
using GameLovers;
using GameLovers.ConfigsContainer;
using GameLovers.Services;
using I2.Loc;
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
		[SerializeField] private TextMeshProUGUI _nameText;
		[SerializeField] private TextMeshProUGUI _sliderText;
		[SerializeField] private Button _collectButton;
		[SerializeField] private Slider _slider;

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
			_nameText.text = data.IsCompleted
				? ScriptLocalization.General.Collect.ToUpper()
				: LocalizationManager.GetTranslation($"{nameof(ScriptLocalization.Achievements)}/{data.AchievementType}");
			_sliderText.text = $"{data.CurrentValue.ToString()}/{data.Goal.ToString()}";
			_slider.value = (float) data.CurrentValue / data.Goal;
			_collectButton.interactable = data.IsCompleted;
			
			_slider.fillRect.gameObject.SetActive(data.CurrentValue > 0);
		}

		private void OnCompleteClicked()
		{
			_services.CommandService.ExecuteCommand(new CollectAchievementCommand { Achievement = _achievementId });
		}
	}
}