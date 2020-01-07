using GameLovers.ConfigsContainer;
using GameLovers.Services;
using Services;

namespace Logic
{
	/// <summary>
	/// Provides access to all game's data.
	/// This interface provides the data with view only permissions
	/// </summary>
	public interface IGameDataProvider
	{
		/// <inheritdoc cref="IConfigs"/>
		IConfigs Configs { get; }
		
		/// <inheritdoc cref="IEntityDataProvider"/>
		IEntityDataProvider EntityDataProvider { get; }
	}

	/// <summary>
	/// Provides access to all game's logic
	/// This interface shouldn't be exposed to the views or controllers
	/// To interact with the logic, execute a <see cref="Commands.IGameCommand"/> via the <see cref="ICommandService"/>
	/// </summary>
	public interface IGameLogic : IGameDataProvider
	{
	}

	/// <inheritdoc />
	/// <remarks>
	/// This interface is only available internally to other logics
	/// </remarks>
	public interface IGameInternalLogic : IGameLogic
	{
		/// <inheritdoc cref="IDataProviderLogic"/>
		IDataProviderLogic DataProviderLogic { get; }
		
		/// <inheritdoc cref="IEntityLogic"/>
		IEntityLogic EntityLogic { get; }

		/// <inheritdoc cref="IMessageBrokerService"/>
		IMessageBrokerService MessageBrokerService { get; }
	}

	/// <inheritdoc />
	public class GameLogic : IGameInternalLogic
	{
		/// <inheritdoc />
		public IConfigs Configs { get; }

		/// <inheritdoc />
		public IDataProviderLogic DataProviderLogic { get; }

		/// <inheritdoc />
		public IEntityDataProvider EntityDataProvider => EntityLogic;

		/// <inheritdoc />
		public IEntityLogic EntityLogic { get; }

		/// <inheritdoc />
		public IMessageBrokerService MessageBrokerService { get; }

		public GameLogic(IMessageBrokerService messageBroker)
		{
			MessageBrokerService = messageBroker;
			
			Configs = new Configs();
			DataProviderLogic = new DataProviderLogic(this);
			EntityLogic = new EntityLogic(this);
		}
	}
}