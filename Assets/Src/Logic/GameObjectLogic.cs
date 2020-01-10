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
		/// Requests the <see cref="GameObject"/> of the given <paramref name="entity"/>
		/// </summary>
		GameObject GetGameObject(EntityId entity);

		/// <summary>
		/// Requests if the given <paramref name="entity"/> has a <see cref="GameObject"/> component in ti
		/// </summary>
		bool HasGameObject(EntityId entity);
	}

	/// <inheritdoc />
	public interface IGameObjectLogic : IGameObjectDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		Task<GameObject> LoadGameObject(EntityId entity, AddressableId addressable, Vector3 position);

		/// <summary>
		/// TODO:
		/// </summary>
		void UnloadGameObject(EntityId entity);
	}
	
	/// <inheritdoc />
	public class GameObjectLogic : IGameObjectLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<EntityId, GameObjectData> _data;
		
		private GameObjectLogic() {}

		public GameObjectLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
			_data = new Dictionary<EntityId, GameObjectData>();
			
			_gameLogic.MessageBrokerService.Subscribe<EntityDestroyedEvent>(eventData => UnloadGameObject(eventData.Entity));
		}

		/// <inheritdoc />
		public GameObject GetGameObject(EntityId entity)
		{
			return _data[entity].Instance;
		}

		/// <inheritdoc />
		public bool HasGameObject(EntityId entity)
		{
			return _data.ContainsKey(entity);
		}

		/// <inheritdoc />
		public async Task<GameObject> LoadGameObject(EntityId entity, AddressableId addressable, Vector3 position)
		{
			var address = _gameLogic.ConfigsProvider.GetConfig<AddressableConfig>((int) addressable).Address;
			var task = LoaderUtil.LoadAssetAsync<GameObject>(address, false);

			await task;

			// ReSharper disable once AccessToStaticMemberViaDerivedType
			var gameObject = GameObject.Instantiate(task.Result, position, Quaternion.identity);
			
			gameObject.GetComponent<EntityMonoComponent>().Entity = entity;
			
			_data.Add(entity, new GameObjectData
			{
				Instance = gameObject,
				LoadReference = task.Result
			});

			return gameObject;
		}

		/// <inheritdoc />
		public void UnloadGameObject(EntityId entity)
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