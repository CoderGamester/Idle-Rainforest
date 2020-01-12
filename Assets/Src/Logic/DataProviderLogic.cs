using System;
using System.Collections.Generic;
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
	public interface IDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		T GetData<T>() where T : class;
	}
	
	/// <summary>
	/// This logic provides the interface to all game's data in the game.
	/// It is also responsible to save the data so it is persistent for multiple sessions
	/// </summary>
	public interface IDataProviderLogic
	{
		/// <summary>
		/// When this method is invoked, all the data is locally saved
		/// </summary>
		void FlushData();
	}

	/// <inheritdoc cref="IDataProviderLogic" />
	public class DataProviderLogic : IDataProviderLogic, IDataProvider
	{
		private readonly IDictionary<Type, object> _data = new Dictionary<Type, object>();
		
		private DataProviderLogic() {}

		public DataProviderLogic(IMessageBrokerService messageBrokerService)
		{
			messageBrokerService.Subscribe<ApplicationPausedEvent>(OnApplicationPaused);
		}

		public void AddData<T>(T data) where T : class
		{
			_data.Add(typeof(T), data);
		}

		/// <inheritdoc />
		public T GetData<T>() where T : class
		{
			return _data[typeof(T)] as T;
		}

		/// <inheritdoc />
		public void FlushData()
		{
			foreach (var data in _data)
			{
				PlayerPrefs.SetString(data.Key.Name, JsonConvert.SerializeObject(data.Value));
			}
			
			PlayerPrefs.Save();
		}

		private void OnApplicationPaused(ApplicationPausedEvent eventData)
		{
			if (eventData.IsPaused)
			{
				FlushData();
			}
		}
	}
}