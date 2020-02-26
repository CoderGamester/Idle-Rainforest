using System.Collections.Generic;
using System.Threading.Tasks;
using GameLovers.AssetLoader;
using Ids;
using MonoComponent;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IWorldObjectDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		T GetWorldObject<T>() where T : Object;
		
		/// <summary>
		/// TODO:
		/// </summary>
		bool TryGetWorldObject<T>(out UniqueId id, out T worldObject) where T : Object;
		
		/// <summary>
		/// TODO:
		/// </summary>
		T GetWorldObject<T>(UniqueId id) where T : Object;
		
		/// <summary>
		/// TODO:
		/// </summary>
		bool TryGetWorldObject<T>(UniqueId id, out T worldObject) where T : Object;

		/// <summary>
		/// TODO:
		/// </summary>
		bool HasWorldObject(UniqueId id);
	}

	/// <inheritdoc />
	public interface IWorldObjectLogic : IWorldObjectDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void AddWorldObject<T>(UniqueId id, T worldObject) where T : Object;

		/// <summary>
		/// TODO:
		/// </summary>
		void RemoveWorldObject(UniqueId id);

		/// <summary>
		/// TODO:
		/// </summary>
		void RemoveWorldObject<T>(T worldObject) where T : Object;
	}
	
	/// <inheritdoc />
	public class WorldObjectLogic : IWorldObjectLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<UniqueId, Object> _data = new Dictionary<UniqueId, Object>();
		
		private WorldObjectLogic() {}

		public WorldObjectLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}

		/// <inheritdoc />
		public T GetWorldObject<T>() where T : Object
		{
			if (TryGetWorldObject<T>(out var id, out var worldObject))
			{
				return worldObject;
			}
			
			throw new LogicException($"There is no world object with the given type {typeof(T)}");
		}

		/// <inheritdoc />
		public bool TryGetWorldObject<T>(out UniqueId id, out T worldObject) where T : Object
		{
			var type = typeof(T);
			
			foreach (var pair in _data)
			{
				if (pair.Value.GetType().UnderlyingSystemType == type)
				{
					id = pair.Key;
					worldObject = pair.Value as T;

					return true;
				}
			}
			
			id = UniqueId.Invalid;
			worldObject = default;

			return false;
		}

		/// <inheritdoc />
		public T GetWorldObject<T>(UniqueId id) where T : Object
		{
			if (TryGetWorldObject<T>(id, out var worldObject))
			{
				return worldObject;
			}
			
			throw new LogicException($"There is no world object with the given id {id.ToString()}");
		}

		/// <inheritdoc />
		public bool TryGetWorldObject<T>(UniqueId id, out T worldObject) where T : Object
		{
			if (_data.TryGetValue(id, out var referenceObject))
			{
				worldObject = referenceObject as T;
				return true;
			}

			worldObject = default;
			
			return false;
		}

		/// <inheritdoc />
		public bool HasWorldObject(UniqueId id)
		{
			return _data.ContainsKey(id);
		}

		/// <inheritdoc />
		public void AddWorldObject<T>(UniqueId id, T worldObject) where T : Object
		{
			_data.Add(id, worldObject);
		}

		/// <inheritdoc />
		public void RemoveWorldObject(UniqueId id)
		{
			_data.Remove(id);
		}

		/// <inheritdoc />
		public void RemoveWorldObject<T>(T worldObject) where T : Object
		{
			foreach (var pair in _data)
			{
				if (pair.Value == worldObject)
				{
					RemoveWorldObject(pair.Key);
					break;
				}
			}
		}
	}
}