using System.Threading.Tasks;
using Configs;
using GameLovers.AddressableIdsScriptGenerator;
using GameLovers.ConfigsContainer;
using GameLovers.LoaderExtension;
using GameLovers.Statechart;
using GameLovers.UiService;
using Ids;
using Presenters;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main
{
	/// <summary>
	/// This class represents the Loading state in the <seealso cref="GameStateMachine"/>
	/// </summary>
	internal class LoadingState
	{
		private readonly ConfigsProvider _gameConfigs;
		private readonly UiService _uiService;
		
		public LoadingState(ConfigsProvider gameConfigs, UiService uiService)
		{
			_gameConfigs = gameConfigs;
			_uiService = uiService;
		}

		/// <summary>
		/// Executes the initial Loading process when the game starts with the given <paramref name="activity"/> to be
		/// invoked when the loading is complete
		/// </summary>
		public async void InitialLoading(IWaitActivity activity)
		{
			await LoadAddressableConfigs();
			await LoadUiConfigsConfigs();
			await LoadOpenLoadingScreen();
			await LoadConfigs(0.2f);
			await LoadInitialUis(0.6f);
			await LoadGameWorld(0.9f);

			Resources.UnloadUnusedAssets();
			_uiService.GetUi<LoadingScreenPresenter>().SetLoadingPercentage(1);

			// Small delay to give the loading complete feedback
			await Task.Delay(500);

			_uiService.CloseUi<LoadingScreenPresenter>();
			activity.Complete();
		}

		private async Task LoadAddressableConfigs()
		{
			await Addressables.InitializeAsync().Task;
			
			var configs = await LoaderUtil.LoadAssetAsync<AddressableConfigs>($"Configs/{nameof(AddressableConfigs)}.asset", true);
			
			_gameConfigs.AddConfigs(config => config.Id, configs.Configs);
		}

		private async Task LoadUiConfigsConfigs()
		{
			var address = _gameConfigs.GetConfig<AddressableConfig>((int) AddressableId.Configs_UiConfigs).Address;
			var configs = await LoaderUtil.LoadAssetAsync<UiConfigs>(address, true);
			
			_uiService.Init(configs);
		}

		private async Task LoadOpenLoadingScreen()
		{
			await _uiService.LoadUiAsync<LoadingScreenPresenter>();
			
			_uiService.OpenUi<LoadingScreenPresenter>().SetLoadingPercentage(0);
		}

		private async Task LoadConfigs(float loadingCap)
		{
			var buildings = await LoaderUtil.LoadAssetAsync<BuildingConfigs>(
				_gameConfigs.GetConfig<AddressableConfig>((int) AddressableId.Configs_BuildingConfigs).Address, true);
			var cards = await LoaderUtil.LoadAssetAsync<CardConfigs>(
				_gameConfigs.GetConfig<AddressableConfig>((int) AddressableId.Configs_CardConfigs).Address, true);
			
			_gameConfigs.AddConfigs(building => (int) building.Building, buildings.Configs);
			_gameConfigs.AddConfigs(card => (int) card.Id, cards.Configs);
			
			_uiService.GetUi<LoadingScreenPresenter>().SetLoadingPercentage(loadingCap);
		}

		private async Task LoadInitialUis(float loadingCap)
		{
			var loadingScreen = _uiService.GetUi<LoadingScreenPresenter>();
			var tasks = _uiService.LoadUiSetAsync((int) UiSetId.InitialLoadUi);
			var initialLoadingPercentage = loadingScreen.LoadingPercentage;
			var loadingBuffer = tasks.Length / loadingCap - initialLoadingPercentage;
			var loadedUiCount = 0f;

			// Load all
			foreach (var taskTemplate in tasks)
			{
				var task = await taskTemplate;
				var ui = await task;

				loadedUiCount++;

				loadingScreen.SetLoadingPercentage(initialLoadingPercentage + loadedUiCount / loadingBuffer);
				ui.gameObject.SetActive(false);
			}

			loadingScreen.SetLoadingPercentage(loadingCap);
		}

		private async Task LoadGameWorld(float loadingCap)
		{
			var loadingScreen = _uiService.GetUi<LoadingScreenPresenter>();
			
			// TODO: Load world assets
			
			loadingScreen.SetLoadingPercentage(loadingCap);
		}
	}
}