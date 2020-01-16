using GameLovers.Services;
using GameLovers.UiService;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Presenters
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class EventPanelPresenter : UiPresenter
	{
		[SerializeField] private Button _closeButton;
		
		private IGameServices _services;

		private void Awake()
		{
			_services = MainInstaller.Resolve<IGameServices>();
			
			_closeButton.onClick.AddListener(OnCloseClicked);
		}

		private void OnCloseClicked()
		{
			var ui = _services.UiService.UnloadUi<CardsPanelPresenter>();
			
			Addressables.Release(ui.gameObject);
			Destroy(ui.gameObject);
		}
	}
}