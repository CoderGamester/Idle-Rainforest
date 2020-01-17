using Systems;
using Commands;
using Configs;
using Data;
using GameLovers.ConfigsContainer;
using GameLovers.Statechart;
using GameLovers.UiService;
using Ids;
using Logic;
using Presenters;
using Services;
using UnityEngine;

namespace Main
{
	/// <summary>
	/// The State Machine that controls the entire flow of the game
	/// </summary>
	public class GameStateMachine : IStatechart
	{
		private readonly IStatechart _stateMachine;
		private readonly IGameInternalLogic _gameLogic;
		private readonly IGameServices _services;
		private readonly LoadingState _loadingState;

		/// <inheritdoc />
		public bool LogsEnabled
		{
			get => _stateMachine.LogsEnabled;
			set => _stateMachine.LogsEnabled = value;
		}

		public GameStateMachine(IGameInternalLogic gameLogic, IGameServices services)
		{
			_gameLogic = gameLogic;
			_services = services;
			_loadingState = new LoadingState((ConfigsProvider) gameLogic.ConfigsProvider, (UiService) services.UiService);
			_stateMachine = new Statechart(Setup);
		}

		/// <inheritdoc />
		public void Trigger(IStatechartEvent trigger)
		{
			_stateMachine.Trigger(trigger);
		}

		/// <inheritdoc />
		public void Run()
		{
			_stateMachine.Run();
		}

		/// <inheritdoc />
		public void Pause()
		{
			_stateMachine.Pause();
		}

		/// <inheritdoc />
		public void Reset()
		{
			_stateMachine.Reset();
		}

		private void Setup(IStateFactory stateFactory)
		{
			var initial = stateFactory.Initial("Initial");
			var initialLoading = stateFactory.Wait("Initial Loading");
			var game = stateFactory.State("Game");
			
			initial.Transition().Target(initialLoading);
			
			initialLoading.WaitingFor(_loadingState.InitialLoading).Target(game);
			
			game.OnEnter(InitializeGame);
		}

		private async void InitializeGame()
		{
			_services.UiService.OpenUiSet((int) UiSetId.MainUi, false);

			// TODO: Delete horrible code below
			
			var info = _gameLogic.BuildingLogic.GetEventInfo();
			var reset = false;
			
			if (_gameLogic.TimeService.DateTimeUtcNow < info.EndTime && 
			    (_gameLogic.DataProviderLogic.AppData.LastLoginTime < info.StartTime || 
			     _gameLogic.DataProviderLogic.AppData.FirstLoginTime == _gameLogic.DataProviderLogic.AppData.LoginTime))
			{
				reset = true;
				
				_gameLogic.DataProviderLogic.PlayerData.Buildings.Clear();
				_gameLogic.DataProviderLogic.PlayerData.GameIds.Clear();
				_gameLogic.DataProviderLogic.PlayerData.Cards.Clear();
				_gameLogic.DataProviderLogic.CurrencyData.MainCurrency = 0;
				
				var ui = await _services.UiService.LoadUiAsync<EventPanelPresenter>();
				
				ui.gameObject.SetActive(true);
			}
			
			var tickSystem = new AutoCollectSystem(_gameLogic.DataProviderLogic.PlayerData.Buildings);
			_services.TickService.SubscribeOnUpdate(deltaTime => tickSystem.Tick());
			if (reset && _gameLogic.DataProviderLogic.AppData.FirstLoginTime == _gameLogic.DataProviderLogic.AppData.LoginTime)
			{
				var list = _gameLogic.ConfigsProvider.GetConfigsList<BuildingConfig>();
			
				for (var i = 0; i < list.Count; i++)
				{
					_services.CommandService.ExecuteCommand(new CreateBuildingCommand
					{
						BuildingType = list[i].Building,
						Position = i * 5 * Vector3.forward + Vector3.left * 1.5f
					});
				}
			}
			else
			{
				foreach (var buildingData in _gameLogic.DataProviderLogic.PlayerData.Buildings)
				{
					_gameLogic.GameObjectDataProvider.LoadGameObject(buildingData.Id, AddressableId.Prefabs_Building, buildingData.Position);
				}
			}
		}
	}
}