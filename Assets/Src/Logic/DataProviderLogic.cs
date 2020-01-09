using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data;
using Events;
using GameLovers.Services;
using Ids;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IPersistentDataProvider
	{
		/// <inheritdoc cref="AppData"/>
		AppData AppData { get; set; }
		/// <inheritdoc cref="PlayerData"/>
		PlayerData PlayerData { get; set; }
	}

	/// <summary>
	/// TODO:
	/// </summary>
	public interface ISessionDataProvider
	{
		/// <summary>
		/// Requests the current <see cref="EntityId"/> counter
		/// </summary>
		EntityId EntityCounter { get; set; }
		
		/// <summary>
		/// TODO:
		/// </summary>
		IReadOnlyDictionary<Type, IEntityDictionary> SessionData { get; }
		
		/// <summary>
		/// TODO:
		/// </summary>
		IDictionary<EntityId, T> GetSessionData<T>();
	}
	
	/// <summary>
	/// This logic provides the interface to all game's data in the game.
	/// It is also responsible to save the data so it is persistent for multiple sessions
	/// </summary>
	public interface IDataProviderLogic : IPersistentDataProvider
	{
		/// <summary>
		/// When this method is invoked, all the data is locally saved
		/// </summary>
		void FlushData();
	}

	/// <inheritdoc cref="IDataProviderLogic" />
	public class DataProviderLogic : IDataProviderLogic, ISessionDataProvider
	{
		/// <inheritdoc />
		public AppData AppData { get; set; }
		/// <inheritdoc />
		public PlayerData PlayerData { get; set; }

		/// <inheritdoc />
		public EntityId EntityCounter { get; set; }

		/// <inheritdoc />
		public IReadOnlyDictionary<Type, IEntityDictionary> SessionData { get; private set; }
		
		private DataProviderLogic() {}

		public DataProviderLogic(IMessageBrokerService messageBrokerService)
		{
			LoadData();
			
			messageBrokerService.Subscribe<ApplicationPausedEvent>(OnApplicationPauseEvent);
		}

		/// <inheritdoc />
		public IDictionary<EntityId, T> GetSessionData<T>()
		{
			return SessionData[typeof(T)] as IDictionary<EntityId, T>;
		}

		/// <inheritdoc />
		public void FlushData()
		{
			PlayerPrefs.SetString("AppData", JsonConvert.SerializeObject(AppData));
			PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(PlayerData));
			PlayerPrefs.Save();
		}

		private void LoadData()
		{
			var appDataJson = PlayerPrefs.GetString("AppData", "");
			var playerDataJson = PlayerPrefs.GetString("PlayerData", "");
			
			AppData = string.IsNullOrEmpty(appDataJson) ? new AppData() : JsonConvert.DeserializeObject<AppData>(appDataJson);
			PlayerData = string.IsNullOrEmpty(playerDataJson) ? new PlayerData() : JsonConvert.DeserializeObject<PlayerData>(playerDataJson);

			AppData.LastLoginTime = DateTime.Now;

			SetSessionData();
		}

		private void SetSessionData()
		{
			var sessionData = new Dictionary<Type, IEntityDictionary>();
			var gameIdDictionary = new EntityDictionary<GameId>();
			var buildingDictionary = new EntityDictionary<int>();

			for (var i = 0; i < PlayerData.Buildings.Count; i++)
			{
				var entity = EntityCounter++;

				buildingDictionary.Add(entity, i);
				gameIdDictionary.Add(entity, PlayerData.Buildings[i].GameId);
			}

			sessionData.Add(typeof(int), buildingDictionary);
			sessionData.Add(typeof(GameId), gameIdDictionary);
			
			SessionData = new ReadOnlyDictionary<Type, IEntityDictionary>(sessionData);
		}

		private void OnApplicationPauseEvent(ApplicationPausedEvent eventData)
		{
			if (eventData.IsPaused)
			{
				FlushData();
			}
		}
	}
}