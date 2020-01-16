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

		// TODO: Event Start Screen
		// TODO: Event countdown timer
		// TODO: Rename Buildings to LevelBuildings/Level and make it mapped by GameId and not UniqueId
		// TODO: AutoCollectSystem optimize
		// TODO: LoadData in GameLogic or LoadingState???
		// TODO: GameLogic custom exception
		
		// TODO: Try IdListResolver without Func<List> and use List reference directly
		// TODO: Select Google Sheet asset
		// TODO: Select UiConfigs asset
		// TODO: UiService Unload also removes Ui from visibleUiList. Rename method to "Remove" and add proper Unload
		// TODO: UiService Close(this);
		// TODO: UiPresenter protected Close() method
		// TODO: GoogleSheet Random(2 > 4), RandomInt, RandomFloat
		// TODO: KeyValuePair Serialization on CsvParser
		// TODO: IdResolver in IConfig
		// TODO: Enum serialize as string
		// TODO: Have possibility of independent pool outside of the PoolService
		// TODO: Make the UiPresenter.Refresh() a public virtual method and not executed by the UiService
	}
}
