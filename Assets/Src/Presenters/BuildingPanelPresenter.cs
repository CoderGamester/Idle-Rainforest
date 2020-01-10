using GameLovers.UiService;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
	public class BuildingPanelPresenter : UiPresenter
	{
		[SerializeField] private Button _upgradeButton;

		private void Awake()
		{
			_upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
		}

		private void OnUpgradeButtonClicked()
		{
			Debug.Log("building panel upgrade button clicked");
		}
	}
}