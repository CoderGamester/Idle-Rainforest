using System.Collections.Generic;
using System.Threading.Tasks;
using Events;
using GameLovers.AddressableIdsScriptGenerator;
using GameLovers.LoaderExtension;
using Ids;
using MonoComponent;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IGameObjectDataProvider
	{
		/// <summary>
		/// Requests the <see cref="GameObject"/> of the given <paramref name="id"/>
		/// </summary>
		GameObject GetGameObject(UniqueId id);

		/// <summary>
		/// Requests if the given <paramref name="id"/> has a <see cref="GameObject"/> component in ti
		/// </summary>
		bool HasGameObject(UniqueId id);
	}

	/// <inheritdoc />
	public interface IGameObjectLogic : IGameObjectDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		Task<GameObject> LoadGameObject(UniqueId id, AddressableId addressable, Vector3 position);

		/// <summary>
		/// TODO:
		/// </summary>
		void UnloadGameObject(UniqueId id);
	}
	
	/// <inheritdoc />
	public class GameObjectLogic : IGameObjectLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<UniqueId, GameObjectData> _data;
		
		private GameObjectLogic() {}

		public GameObjectLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
			_data = new Dictionary<UniqueId, GameObjectData>();
		}

		/// <inheritdoc />
		public GameObject GetGameObject(UniqueId id)
		{
			return _data[id].Instance;
		}

		/// <inheritdoc />
		public bool HasGameObject(UniqueId id)
		{
			return _data.ContainsKey(id);
		}

		/// <inheritdoc />
		public async Task<GameObject> LoadGameObject(UniqueId id, AddressableId addressable, Vector3 position)
		{
			var address = _gameLogic.ConfigsProvider.GetConfig<AddressableConfig>((int) addressable).Address;
			var task = LoaderUtil.LoadAssetAsync<GameObject>(address, false);

			await task;

			// ReSharper disable once AccessToStaticMemberViaDerivedType
			var gameObject = GameObject.Instantiate(task.Result, position, Quaternion.identity);
			
			gameObject.GetComponent<EntityMonoComponent>().UniqueId = id;
			
			_data.Add(id, new GameObjectData
			{
				Instance = gameObject,
				LoadReference = task.Result
			});

			return gameObject;
		}

		/// <inheritdoc />
		public void UnloadGameObject(UniqueId entity)
		{
			var gameObjectData = _data[entity];
			
			Object.Destroy(gameObjectData.Instance);
			Addressables.Release(gameObjectData.LoadReference);

			_data.Remove(entity);
		}
		
		private struct GameObjectData
		{
			public GameObject Instance;
			public GameObject LoadReference;
		}
	}
}