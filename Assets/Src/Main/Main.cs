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
		
		// TODO: Character cards and soft currency bonus at main intervals, 10, 10, 10, then 50 ongoing
		// TODO: Automation system
		// TODO; Three animal characters (Cards) - one for each area
		// TODO: Character upgrades - upgrades increase production rate by: 1st upgrade = 2x, 2nd ugrade = 5x, 3rd upgrde = 10x 
		
		// TODO: IdResolver in IConfig
		// TODO: Enum serialize as string
	}
}
