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
using UnityEngine.InputSystem.UI;

namespace Main
{
	/// <summary>
	/// The Main entry point of the game
	/// </summary>
	public class Main : MonoBehaviour
	{
		public Camera MainCamera;
		public InputSystemUIInputModule InputSystem;
		
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
			_gameServices = new GameServices(this, messageBroker, timeService, _gameLogic);
			
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

		// TODO: New Tree menu to upgrade trees
		// TODO: Change LevelTrees Data structure
		// TODO: Have CreateBuilding only when the building is created not in the beginning of the game
		
		// TODO: DataProvider contains all IdLists and remove public achievement list
		// TODO: AutoCollectSystem optimize
		// TODO: GameObjectLogic is confusing. Adding reference UniqueId -> GameObject should be enough
		// TODO: Systems Architecture. Use Unity DOTS???
		// TODO: Boxes for rewards
		// TODO: Show Achievements Completed UI and reset data when completing all achievements
		// TODO: StartEventCommand that resets all the data and recreates all the world
		// TODO: Asset Loading management. Check sprites loading on the trees/animals
		
		// TODO: 5 < 8 -> RandomInt(5,8)
		// TODO: 5.1 < 8.1 -> RandomFloat(5,8)
		// TODO: Improve CSVParser to Deserialize IntData, FloatData, IntPair & FloatPair
		// TODO: Improve CSVParser to Deserialize list of IntData, FloatData, IntPair & FloatPair
		// TODO: Enum serialize as string
		// TODO: Async Await working
		// TODO: Missing RuntimeTests & EditorTests
	}
}
