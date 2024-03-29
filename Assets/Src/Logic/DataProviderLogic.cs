using System;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IDataProvider
	{
		/// <inheritdoc cref="AppData" />
		AppData AppData { get; }
		/// <inheritdoc cref="PlayerData" />
		PlayerData PlayerData { get; }
		/// <inheritdoc cref="CurrencyData" />
		CurrencyData CurrencyData { get; }
		/// <inheritdoc cref="LevelData" />
		LevelData LevelData { get; }
	}
	
	/// <summary>
	/// This logic provides the interface to all game's data in the game.
	/// It is also responsible to save the data so it is persistent for multiple sessions
	/// </summary>
	public interface IDataProviderLogic
	{
		/// <summary>
		/// Saves all game's data locally
		/// </summary>
		void FlushData();
		
		/// <summary>
		/// Saves the game's given <typeparamref name="T"/> data locally
		/// </summary>
		void FlushData<T>() where T : class;
	}

	/// <inheritdoc cref="IDataProviderLogic" />
	/// <remarks>
	/// Allows to add data 
	/// </remarks>
	public interface IDataProviderInternalLogic : IDataProviderLogic, IDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		T GetData<T>() where T : class;
		
		/// <summary>
		/// TODO:
		/// </summary>
		void AddData<T>(T data) where T : class;
	}

	/// <inheritdoc cref="IDataProviderLogic" />
	public class DataProviderLogic : IDataProviderInternalLogic
	{
		private readonly IDictionary<Type, object> _data = new Dictionary<Type, object>();

		/// <inheritdoc />
		public AppData AppData => GetData<AppData>();
		/// <inheritdoc />
		public PlayerData PlayerData => GetData<PlayerData>();
		/// <inheritdoc />
		public CurrencyData CurrencyData => GetData<CurrencyData>();
		/// <inheritdoc />
		public LevelData LevelData => GetData<LevelData>();

		/// <inheritdoc />
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

		/// <inheritdoc />
		public void FlushData<T>() where T : class
		{
			var type = typeof(T);
			
			PlayerPrefs.SetString(type.Name, JsonConvert.SerializeObject(_data[type]));
			PlayerPrefs.Save();
		}
	}
}