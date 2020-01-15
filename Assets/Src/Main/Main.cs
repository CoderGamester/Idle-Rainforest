using System;
using Events;
using GameLovers.Services;
using Logic;
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
			
			_gameLogic = new GameLogic(messageBroker, timeService);
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
		}

		private void OnApplicationQuit()
		{
			_gameLogic.DataProviderLogic.FlushData();
		}

		// TODO: BuildingMonoComponent todos
		// TODO: UI
		// TODO: AutoCollectSystem optimize
		
		// TODO: Try IdListResolver without Func<List> and use List reference directly
		// TODO: Rename Buildings to Level and make it mapped by GameId and not UniqueId
		// TODO: Expose Persistent data types in the DataProviderLogic/GameLogic and check IGamerInternalLogic need
		// TODO: GameLogic custom exception
		// TODO: Select Google Sheet asset
		// TODO: UiService Close(this);
		// TODO: GoogleSheet Random(2 > 4), RandomInt, RandomFloat
		// TODO: KeyValuePair Serialization on CsvParser
		// TODO: IdResolver in IConfig
		// TODO: Enum serialize as string
	}
}
