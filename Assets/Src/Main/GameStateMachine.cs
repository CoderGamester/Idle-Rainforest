using Commands;
using Configs;
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
			game.OnEnter(InitializeEvent);
		}

		private void InitializeEvent()
		{
			var info = _gameLogic.BuildingLogic.GetEventInfo();
			
			if (_gameLogic.DataProviderLogic.AppData.LastLoginTime < info.StartTime &&
			    _gameLogic.TimeService.DateTimeUtcNow < info.EndTime)
			{
				_services.UiService.LoadUiAsync<EventPanelPresenter>().ContinueWith(task => task.Result.gameObject.SetActive(true));
			}
		}

		private void InitializeGame()
		{
			var list = _gameLogic.ConfigsProvider.GetConfigsList<BuildingConfig>();
			
			for (var i = 0; i < list.Count; i++)
			{
				_services.CommandService.ExecuteCommand(new CreateBuildingCommand
				{
					BuildingType = list[i].Building,
					Position = i * 5 * Vector3.forward
				});
			}
			
			_services.UiService.OpenUiSet((int) UiSetId.MainUi, false);
		}
	}
}