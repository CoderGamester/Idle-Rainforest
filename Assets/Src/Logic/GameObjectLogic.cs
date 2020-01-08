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
	}

	/// <inheritdoc />
	public interface IGameObjectLogic : IGameObjectDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		Task<GameObject> LoadGameObject(EntityId entity, GameId gameObject, Vector3 position);

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
			
			_gameLogic.MessageBrokerService.Subscribe<EntityDestroyEvent>(eventData => UnloadGameObject(eventData.Entity));
		}

		/// <inheritdoc />
		public GameObject GetGameObject(EntityId entity)
		{
			return _data[entity].Instance;
		}

		/// <inheritdoc />
		public async Task<GameObject> LoadGameObject(EntityId entity, GameId gameId, Vector3 position)
		{
			var address = _gameLogic.Configs.GetConfig<AddressableConfig>((int) gameId).Address;
			var task = LoaderUtil.LoadAssetAsync<GameObject>(address, false);

			await task;

			var instance = Object.Instantiate(task.Result, position, Quaternion.identity);
			
			instance.GetComponent<EntityMonoComponent>().Entity = entity;
			
			_data.Add(entity, new GameObjectData
			{
				Instance = instance,
				LoadReference = task.Result
			});

			return instance;
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