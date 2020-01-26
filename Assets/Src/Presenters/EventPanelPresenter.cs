using System;
using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class EventPanelPresenter : UiPresenter
	{
		[SerializeField] private TextMeshProUGUI _timeText;
		[SerializeField] private Button _closeButton;
		
		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_services.TickService.SubscribeOnUpdate(UpdateCountdown, 1, true);
			_closeButton.onClick.AddListener(OnCloseClicked);
		}

		private void UpdateCountdown(float deltaTime)
		{
			var info = _dataProvider.EventDataProvider.GetEventInfo();
			
			_timeText.text = $"Time for the event to end:\n{(info.EndTime - DateTime.UtcNow).ToString(@"hh\:mm\:ss")}";
		}

		private void OnCloseClicked()
		{
			_services.UiService.UnloadUi<EventPanelPresenter>();
		}
	}
}