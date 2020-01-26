using System;
using System.Collections.Generic;
using Systems;
using Data;
using Events;
using GameLovers.Services;
using Logic;
using Newtonsoft.Json;
using Services;
using UnityEngine;

namespace Main
{
	/// <summary>
	/// The Main entry point of the game
	/// </summary>
	public class Main : MonoBehaviour
	{
		private GameStateMachine _stateMachine;
		private GameLogic _gameLogic;
		private GameServices _gameServices;

		private void Awake()
		{
			var messageBroker = new MessageBrokerService();
			var timeService = new TimeService();
			var dataProvider = new DataProviderLogic();
			
			LoadData(dataProvider, timeService);
			
			_gameLogic = new GameLogic(messageBroker, dataProvider, timeService);
			_gameServices = new GameServices(messageBroker, timeService, _gameLogic);
			
			MainInstaller.Bind<IGameDataProvider>(_gameLogic);
			MainInstaller.Bind<IGameServices>(_gameServices);
			
			_stateMachine = new GameStateMachine(_gameLogic, _gameServices);
		}

		private void Start()
		{
			_stateMachine.Run();
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			_gameServices.MessageBrokerService.Publish(new ApplicationPausedEvent { IsPaused = pauseStatus });

			if (pauseStatus)
			{
				_gameLogic.DataProviderLogic.FlushData();
			}
		}

		private void OnApplicationQuit()
		{
			_gameLogic.DataProviderLogic.FlushData();
		}

		private void LoadData(IDataProviderInternalLogic dataProviderLogic, ITimeService timeService)
		{
			var time = timeService.DateTimeUtcNow;
			var appDataJson = PlayerPrefs.GetString(nameof(AppData), "");
			var playerDataJson = PlayerPrefs.GetString(nameof(PlayerData), "");
			var currencyDataJson = PlayerPrefs.GetString(nameof(CurrencyData), "");
			var levelDataJson = PlayerPrefs.GetString(nameof(LevelData), "");
			
			dataProviderLogic.AddData(string.IsNullOrEmpty(appDataJson) ? new AppData() : JsonConvert.DeserializeObject<AppData>(appDataJson));
			dataProviderLogic.AddData(string.IsNullOrEmpty(playerDataJson) ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(playerDataJson));
			dataProviderLogic.AddData(string.IsNullOrEmpty(currencyDataJson) ? new CurrencyData() : JsonConvert.DeserializeObject<CurrencyData>(currencyDataJson));
			dataProviderLogic.AddData(string.IsNullOrEmpty(levelDataJson) ? new LevelData() : JsonConvert.DeserializeObject<LevelData>(levelDataJson));

			if (string.IsNullOrEmpty(appDataJson))
			{
				dataProviderLogic.AppData.FirstLoginTime = time;
				dataProviderLogic.AppData.LoginTime = time;
			}
			
			dataProviderLogic.AppData.LastLoginTime = dataProviderLogic.AppData.LoginTime;
			dataProviderLogic.AppData.LoginTime = time;
			dataProviderLogic.AppData.LoginCount++;
		}

		// TODO: Move achievementsystem to achievementlogic
		// TODO: Show Achievements Completed UI and reset data when completing all achievements
		// TODO: Do the TODOS in GameStateMachine
		// TODO: StartEventCommand that resets all the data and recreates all the world
		// TODO: Remove create gameobject from CreateBuildingCommand
		// TODO: AutoCollectSystem optimize
		// TODO: GameObjectLogic is confusing. Adding reference UniqueId -> GameObject should be enough
		// TODO: GameLogic custom exception
		// TODO: Systems Architecture. Use Unity DOTS???
		// TODO: Add Time cheat and clear all playerprefs into SRDebugger
		// TODO: Change card upgrade cost to soft currency (CardViewPresenter && CardLogic)
		// TODO: DataProvider contains all IdLists and remove public achievement list
		
		// TODO: MainInstaller receives T type as arguemnt
		// TODO: GoogleSheet Random(2 > 4), RandomInt, RandomFloat
		// TODO: Enum serialize as string
		// TODO: Async Await working
		// TODO: Move Frameworks to GitHub
		// TODO: Missing RuntimeTests for services
		// TODO: InspectorGUI on Addressables like UiConfig
		// TODO: Review Addressables configs. remove cache
		// TODO: Review PoolService. Remove service and remove new() constraint and fix despawnall. Clear == Despawnall???. Generic GameObject Instantiator
		// TODO: IdList Count
		// TODO: IntList for an Observable IList
		// TODO: IdDictionary
		// TODO: UIService Unload(this)
		// TODO: UiService Load(bool openAfter = false);
		// TODO: UiService Add(bool openAfter = falsE);
	}
}
