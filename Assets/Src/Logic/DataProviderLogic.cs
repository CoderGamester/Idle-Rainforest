using System;
using Data;
using Events;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic
{
	/// <summary>
	/// This logic provides the interface to all game's data in the game.
	/// It is also responsible to save the data so it is persistent for multiple sessions
	/// </summary>
	public interface IDataProviderLogic
	{
		/// <inheritdoc cref="AppData"/>
		AppData AppData { get; set; }
		/// <inheritdoc cref="PlayerData"/>
		PlayerData PlayerData { get; set; }

		/// <summary>
		/// When this method is invoked, all the data is locally saved
		/// </summary>
		void FlushData();
	}
	
	/// <inheritdoc />
	public class DataProviderLogic : IDataProviderLogic
	{
		/// <inheritdoc />
		public AppData AppData { get; set; }

		/// <inheritdoc />
		public PlayerData PlayerData
		{
			get => _playerData;
			set
			{
				_playerData = value;
				FlushData();
			}
		}
		
		private readonly IGameInternalLogic _gameLogic;

		private PlayerData _playerData;
		
		private DataProviderLogic() {}

		public DataProviderLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
			
			LoadData();
			_gameLogic.MessageBrokerService.Subscribe<ApplicationPauseEvent>(OnApplicationPauseEvent);
		}

		/// <inheritdoc />
		public void FlushData()
		{
			PlayerPrefs.SetInt("HasData", 1);
			PlayerPrefs.SetString("AppData", JsonConvert.SerializeObject(AppData));
			PlayerPrefs.SetString("PlayerData", JsonConvert.SerializeObject(_playerData));
			PlayerPrefs.Save();
		}

		private void LoadData()
		{
			if (!PlayerPrefs.HasKey("HasData"))
			{
				SetDefaultData();
				
				return;
			}
			
			AppData = JsonConvert.DeserializeObject<AppData>(PlayerPrefs.GetString("AppData", ""));
			_playerData = JsonConvert.DeserializeObject<PlayerData>(PlayerPrefs.GetString("PlayerData", ""));

			AppData.LastLoginTime = DateTime.Now;
		}

		private void SetDefaultData()
		{
			AppData = new AppData
			{
				FirstLoginTime = DateTime.Now,
				LastLoginTime = DateTime.Now
			};
			
			_playerData = new PlayerData();
		}

		private void OnApplicationPauseEvent(ApplicationPauseEvent eventData)
		{
			if (eventData.IsPaused)
			{
				FlushData();
			}
		}
	}
}