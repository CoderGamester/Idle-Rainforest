using GameLovers.UiService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presenters
{
	public class LoadingScreenPresenter : UiPresenter
	{
		[SerializeField] private Slider _loadingBar;
		[SerializeField] private TextMeshProUGUI _loadingText;

		public float LoadingPercentage => _loadingBar.value;

		public void SetLoadingPercentage(float percentage)
		{
			_loadingBar.value = percentage;
		}

		protected override void OnOpened()
		{
			_loadingBar.value = 0;
		}
	}
}