using Commands;
using GameLovers.LoaderExtension;
using GameLovers.Services;
using GameLovers.UiService;
using Ids;
using Infos;
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
	public class AutomatePopUpPresenter : UiPresenterData<ulong>
	{
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _automateButton;
		[SerializeField] private Image _image;
		[SerializeField] private TextMeshProUGUI _automateCostText;
		[SerializeField] private TextMeshProUGUI _requirementText;

		private IGameDataProvider _dataProvider;
		private IGameServices _services;
		
		private void Awake()
		{
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_services = MainInstaller.Resolve<IGameServices>();
			
			_closeButton.onClick.AddListener(Close);
			_automateButton.onClick.AddListener(OnAutomateClicked);
		}

		/// <inheritdoc />
		protected override async void OnOpened()
		{
			var info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(Data);

			_image.sprite = await LoaderUtil.LoadAssetAsync<Sprite>($"{AddressablePathLookup.SpritesAnimals}/{info.AutomateCardRequirement.GameId}.png", false);
			_automateCostText.text = $"{info.AutomateCost.ToString()} MC";
			_requirementText.text = $"Requires {info.AutomateCardRequirement.GameId} at level {info.AutomateCardRequirement.Value} to automate";

			_automateButton.interactable = info.AutomationState == AutomationState.ReadyToAutomate;
		}

		private void OnAutomateClicked()
		{
			_services.CommandService.ExecuteCommand(new AutomateBuildingCommand { BuildingId = Data });
		}
	}
}