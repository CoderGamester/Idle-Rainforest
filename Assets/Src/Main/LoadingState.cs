using System.Collections.Generic;
using System.Threading.Tasks;
using Commands;
using Configs;
using GameLovers.AddressableIdsScriptGenerator;
using GameLovers.ConfigsContainer;
using GameLovers.LoaderExtension;
using GameLovers.Statechart;
using GameLovers.UiService;
using Ids;
using Logic;
using Presenters;
using Services;
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
		private readonly IGameInternalLogic _gameLogic;
		
		public LoadingState(ConfigsProvider gameConfigs, UiService uiService, IGameInternalLogic gamelogic)
		{
			_gameConfigs = gameConfigs;
			_uiService = uiService;
			_gameLogic = gamelogic;
		}

		/// <summary>
		/// Executes the initial Loading process when the game starts with the given <paramref name="activity"/> to be
		/// invoked when the loading is complete
		/// </summary>
		public async void InitialLoading(IWaitActivity activity)
		{
			await LoadUiConfigsConfigs();
			await LoadOpenLoadingScreen();
			await LoadConfigs(0.2f);
			await LoadInitialUis(0.6f);
			
			activity.Complete();
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public async void FinalLoading(IWaitActivity activity)
		{
			await LoadGameWorld(1f);

			Resources.UnloadUnusedAssets();

			// Small delay to give the loading complete feedback
			await Task.Delay(500);

			_uiService.CloseUi<LoadingScreenPresenter>();
			activity.Complete();
		}

		private async Task LoadUiConfigsConfigs()
		{
			var configs = await LoaderUtil.LoadAssetAsync<UiConfigs>(AddressableId.Configs_UiConfigs.GetConfig().Address, true);
			
			_uiService.Init(configs);
		}

		private async Task LoadOpenLoadingScreen()
		{
			await _uiService.LoadUiAsync<LoadingScreenPresenter>();
			
			_uiService.OpenUi<LoadingScreenPresenter>().SetLoadingPercentage(0);
		}

		private async Task LoadConfigs(float loadingCap)
		{
			var levelTrees = await LoaderUtil.LoadAssetAsync<LevelTreeConfigs>(AddressableId.Configs_LevelTreeConfigs.GetConfig().Address, true);
			var cards = await LoaderUtil.LoadAssetAsync<CardConfigs>(AddressableId.Configs_CardConfigs.GetConfig().Address, true);
			
			_gameConfigs.AddConfigs(tree => (int) tree.Tree, levelTrees.Configs);
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

			// Load all initial uis
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
			var taskList = new List<Task<GameObject>>();
			
			foreach (var buildingData in _gameLogic.DataProviderInternalLogic.LevelData.Buildings)
			{
				taskList.Add(_gameLogic.GameObjectLogic.LoadGameObject(buildingData.Id, AddressableId.Prefabs_Tree, buildingData.Position));
			}

			foreach (var task in LoaderUtil.Interleaved(taskList))
			{
				await await task;
			}
			
			loadingScreen.SetLoadingPercentage(loadingCap);
		}
	}
}