using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using GameLovers.ConfigsContainer;
using GameLovers.Services;
using Ids;
using Newtonsoft.Json;
using Services;
using UnityEngine;

namespace Logic
{
	/// <summary>
	/// Provides access to all game's data.
	/// This interface provides the data with view only permissions
	/// </summary>
	public interface IGameDataProvider
	{
		/// <inheritdoc cref="IConfigsProvider"/>
		IConfigsProvider ConfigsProvider { get; }
		
		/// <inheritdoc cref="IEntityDataProvider"/>
		IEntityDataProvider EntityDataProvider { get; }
		/// <inheritdoc cref="IGameObjectDataProvider"/>
		IGameObjectDataProvider GameObjectDataProvider { get; }
		/// <inheritdoc cref="IGameIdDataProvider"/>
		IGameIdDataProvider GameIdDataProvider { get; }
		/// <inheritdoc cref="ICurrencyDataProvider"/>
		ICurrencyDataProvider CurrencyDataProvider { get; }
		/// <inheritdoc cref="IBuildingDataProvider"/>
		IBuildingDataProvider BuildingDataProvider { get; }
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
		
		/// <inheritdoc cref="IEntityLogic"/>
		IEntityLogic EntityLogic { get; }
		/// <inheritdoc cref="IGameObjectLogic"/>
		IGameObjectLogic GameObjectLogic { get; }
		/// <inheritdoc cref="IGameIdLogic"/>
		IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc cref="ICurrencyLogic"/>
		ICurrencyLogic CurrencyLogic { get; }
		/// <inheritdoc cref="IBuildingLogic"/>
		IBuildingLogic BuildingLogic { get; }
	}

	/// <inheritdoc />
	/// <remarks>
	/// This interface is only available internally to other logics
	/// </remarks>
	public interface IGameInternalLogic : IGameLogic
	{
		/// <inheritdoc cref="IDataProviderLogic"/>
		IDataProviderLogic DataProviderLogic { get; }
	}

	/// <inheritdoc />
	public class GameLogic : IGameInternalLogic
	{
		/// <inheritdoc />
		public IConfigsProvider ConfigsProvider { get; }
		
		/// <inheritdoc />
		public IMessageBrokerService MessageBrokerService { get; }
		/// <inheritdoc />
		public ITimeService TimeService { get; }

		/// <inheritdoc />
		public IDataProviderLogic DataProviderLogic { get; }
		
		/// <inheritdoc />
		public IEntityDataProvider EntityDataProvider => EntityLogic;
		/// <inheritdoc />
		public IGameObjectDataProvider GameObjectDataProvider => GameObjectLogic;
		/// <inheritdoc />
		public IGameIdDataProvider GameIdDataProvider => GameIdLogic;
		/// <inheritdoc />
		public ICurrencyDataProvider CurrencyDataProvider => CurrencyLogic;
		/// <inheritdoc />
		public IBuildingDataProvider BuildingDataProvider => BuildingLogic;

		/// <inheritdoc />
		public IEntityLogic EntityLogic { get; }
		/// <inheritdoc />
		public IGameObjectLogic GameObjectLogic { get; }
		/// <inheritdoc />
		public IGameIdLogic GameIdLogic { get; }
		/// <inheritdoc />
		public ICurrencyLogic CurrencyLogic { get; }
		/// <inheritdoc />
		public IBuildingLogic BuildingLogic { get; }

		public GameLogic(IMessageBrokerService messageBroker, ITimeService timeService)
		{
			var dataProviderLogic = new DataProviderLogic(messageBroker);
			
			LoadData(dataProviderLogic);
			
			ConfigsProvider = new ConfigsProvider();
			MessageBrokerService = messageBroker;
			TimeService = timeService;
			
			DataProviderLogic = dataProviderLogic;
			EntityLogic = new EntityLogic(this, dataProviderLogic);
			GameObjectLogic = new GameObjectLogic(this);
			CurrencyLogic = new CurrencyLogic(this, dataProviderLogic.GetData<CurrencyData>());
			GameIdLogic = new GameIdLogic(this, new UniqueIdList<GameIdData>(
				data => data.Id, 
				() => dataProviderLogic.GetData<PlayerData>().GameIds));
			BuildingLogic = new BuildingLogic(this, new UniqueIdList<BuildingData>(
				data => data.Id, 
				() => dataProviderLogic.GetData<PlayerData>().Buildings));
		}

		private void LoadData(DataProviderLogic dataProviderLogic)
		{
			var appDataJson = PlayerPrefs.GetString(nameof(AppData), "");
			var playerDataJson = PlayerPrefs.GetString(nameof(PlayerData), "");
			var currencyDataJson = PlayerPrefs.GetString(nameof(CurrencyData), "");
			
			dataProviderLogic.AddData(string.IsNullOrEmpty(appDataJson) ? new AppData() : JsonConvert.DeserializeObject<AppData>(appDataJson));
			dataProviderLogic.AddData(string.IsNullOrEmpty(playerDataJson) ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(playerDataJson));
			dataProviderLogic.AddData(string.IsNullOrEmpty(currencyDataJson) ? new CurrencyData() : JsonConvert.DeserializeObject<CurrencyData>(currencyDataJson));

			dataProviderLogic.GetData<AppData>().LastLoginTime = DateTime.Now;
		}
	}
}