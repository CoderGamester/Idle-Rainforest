using GameLovers.Services;
using GameLovers.UiService;
using Logic;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CardsPanelPresenter : UiPresenter
	{
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _upgradeButton;
		
		private IGameDataProvider _dataProvider;
		private IGameServices _services;

		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_upgradeButton.onClick.AddListener(OnUpgradeClicked);
			_upgradeButton.onClick.AddListener(OnCloseClicked);
		}

		private void OnCloseClicked()
		{
			_services.UiService.CloseUi<CardsPanelPresenter>();
		}

		private void OnUpgradeClicked()
		{
			Debug.Log("building panel upgrade button clicked");
		}
	}
}