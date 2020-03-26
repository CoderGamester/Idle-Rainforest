using Data;
using Data.ComponentData;
using GameLovers;
using GameLovers.ConfigsContainer;
using GameLovers.Services;
using Ids;
using Services;

namespace Logic
{
	/// <summary>
	/// This interface marks the Game Logic as one that needs to initialize it's internal state
	/// </summary>
	public interface IGameLogicInitializer
	{
		/// <summary>
		/// Initializes the Game Logic state to it's default initial values
		/// </summary>
		void Init();
	}
	
	/// <summary>
	/// Provides access to all game's data.
	/// This interface provides the data with view only permissions
	/// </summary>
	public interface IGameDataProvider
	{
		/// <summary>
		/// Requests the information if the current game session is the first time the player is playing the game or not
		/// </summary>
		bool IsFirstSession { get; }
		
		/// <inheritdoc cref="IConfigsProvider"/>
		IConfigsProvider ConfigsProvider { get; }
		
		/// <inheritdoc cref="IRewardDataProvider"/>
		IRewardDataProvider RewardDataProvider { get; }
		/// <inheritdoc cref="IEntityDataProvider"/>
		IEntityDataProvider EntityDataProvider { get; }
		/// <inheritdoc cref="IGameIdDataProvider"/>
		IGameIdDataProvider GameIdDataProvider { get; }
		/// <inheritdoc cref="ICurrencyDataProvider"/>
		ICurrencyDataProvider CurrencyDataProvider { get; }
		/// <inheritdoc cref="IEventDataProvider"/>
		IEventDataProvider EventDataProvider { get; }
		/// <inheritdoc cref="ILevelTreeDataProvider"/>
		ILevelTreeDataProvider LevelTreeDataProvider { get; }
		/// <inheritdoc cref="ICardDataProvider"/>
		ICardDataProvider CardDataProvider { get; }
		/// <inheritdoc cref="IAchievementDataProvider"/>
		IAchievementDataProvider AchievementDataProvider { get; }
	}

	/// <summary>
	/// Provides access to all game's logic
	/// This interface shouldn't be exposed to the views or controllers
	/// To interact with the logic, execute a <see cref="Commands.IGameCommand"/> via the <see cref="ICommandService"/>
	/// </summary>
	public interface IGameLogic : IGameDataProvider
	{
		/// <inheritdoc cref="IMessageBrokerService"/>
		IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc cref="ITimeService"/>
		ITimeService TimeService { get; }
		
		/// <inheritdoc cref="IDataProviderLogic"/>
		IDataProviderLogic DataProviderLogic { get; }
		/// <inheritdoc cref="IEntityLogic"/>
		IEntityLogic EntityLogic { get; }
		/// <inheritdoc cref="IGameIdLogic"/>
		IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc cref="ICurrencyLogic"/>
		ICurrencyLogic CurrencyLogic { get; }
		/// <inheritdoc cref="IEventLogic"/>
		IEventLogic EventLogic { get; }
		/// <inheritdoc cref="ILevelTreeLogic"/>
		ILevelTreeLogic LevelTreeLogic { get; }
		/// <inheritdoc cref="ICardLogic"/>
		ICardLogic CardLogic { get; }
		/// <inheritdoc cref="IAchievementLogic"/>
		IAchievementLogic AchievementLogic { get; }
	}

	/// <inheritdoc cref="IGameLogic" />
	/// <remarks>
	/// This interface is only available internally to other logics
	/// </remarks>
	public interface IGameInternalLogic : IGameLogic, IGameLogicInitializer
	{
		/// <inheritdoc cref="IDataProviderLogic"/>
		IDataProviderInternalLogic DataProviderInternalLogic { get; }
		/// <inheritdoc cref="IRewardLogic"/>
		IRewardLogic RewardLogic { get; }
	}

	/// <inheritdoc />
	public class GameLogic : IGameInternalLogic
	{
		/// <inheritdoc />
		public bool IsFirstSession => DataProviderInternalLogic.AppData.LoginCount == 1;

		/// <inheritdoc />
		public IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc />
		public ITimeService TimeService { get; }

		/// <inheritdoc />
		public IConfigsProvider ConfigsProvider { get; }

		/// <inheritdoc />
		public IRewardDataProvider RewardDataProvider => RewardLogic;
		/// <inheritdoc />
		public IEntityDataProvider EntityDataProvider => EntityLogic;
		/// <inheritdoc />
		public IGameIdDataProvider GameIdDataProvider => GameIdLogic;
		/// <inheritdoc />
		public ICurrencyDataProvider CurrencyDataProvider => CurrencyLogic;
		/// <inheritdoc />
		public IEventDataProvider EventDataProvider => EventLogic;

		/// <inheritdoc />
		public ILevelTreeDataProvider LevelTreeDataProvider => LevelTreeLogic;
		/// <inheritdoc />
		public ICardDataProvider CardDataProvider => CardLogic;
		/// <inheritdoc />
		public IAchievementDataProvider AchievementDataProvider => AchievementLogic;

		/// <inheritdoc />
		public IDataProviderLogic DataProviderLogic => DataProviderInternalLogic;
		/// <inheritdoc />
		public IEntityLogic EntityLogic { get; }
		/// <inheritdoc />
		public IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc />
		public ICurrencyLogic CurrencyLogic { get; }
		/// <inheritdoc />
		public IEventLogic EventLogic { get; }
		/// <inheritdoc />
		public ILevelTreeLogic LevelTreeLogic { get; }
		/// <inheritdoc />
		public ICardLogic CardLogic { get; }
		/// <inheritdoc />
		public IAchievementLogic AchievementLogic { get; }

		/// <inheritdoc />
		public IDataProviderInternalLogic DataProviderInternalLogic { get; }
		/// <inheritdoc />
		public IRewardLogic RewardLogic { get; }

		public GameLogic(IMessageBrokerService messageBroker, IDataProviderInternalLogic dataProviderInternalLogic,
			ITimeService timeService)
		{
			MessageBrokerService = messageBroker;
			TimeService = timeService;
			DataProviderInternalLogic = dataProviderInternalLogic;
			
			ConfigsProvider = new ConfigsProvider();
			RewardLogic = new RewardLogic(this);
			EntityLogic = new EntityLogic(this, DataProviderInternalLogic);
			CurrencyLogic = new CurrencyLogic(this, DataProviderInternalLogic.CurrencyData);
			EventLogic = new EventLogic(this);
			GameIdLogic = new GameIdLogic(this, new UniqueIdList<GameIdData>(data => data.Id, DataProviderInternalLogic.PlayerData.GameIds));
			LevelTreeLogic = new LevelTreeLogic(this, new UniqueIdList<LevelTreeData>(data => data.Id, DataProviderInternalLogic.LevelData.Trees));
			CardLogic = new CardLogic(this, new IdList<GameId, CardData>(data => data.Id, DataProviderInternalLogic.PlayerData.Cards));
			AchievementLogic = new AchievementLogic(this, new UniqueIdList<AchievementData>(x => x.Id, dataProviderInternalLogic.LevelData.Achievements));
		}

		/// <inheritdoc />
		public void Init()
		{
			// ReSharper disable PossibleNullReferenceException
			(AchievementLogic as IGameLogicInitializer).Init();
			(LevelTreeLogic as IGameLogicInitializer).Init();
		}
	}
}