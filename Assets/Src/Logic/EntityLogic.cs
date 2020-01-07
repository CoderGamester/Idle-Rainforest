using System;
using System.Collections.Generic;
using Data;
using Ids;
using UnityEngine;

namespace Logic
{
	/// <summary>
	/// This logic acts as a factory to create or remove all the entities in the game.
	/// It takes care that the right components are added and removed from any game's entities
	/// </summary>
	/// <remarks>
	/// Follows the "Factory Pattern" <see cref="https://en.wikipedia.org/wiki/Factory_method_pattern"/>
	/// </remarks>
	public interface IEntityDataProvider
	{
		/// <summary>
		/// Requests the <see cref="EntityData"/> represented by the given <paramref name="entity"/>
		/// </summary>
		EntityData GetEntityData(UniqueId entity);
	}
	
	/// <inheritdoc />
	public interface IEntityLogic : IEntityDataProvider
	{
		/// <summary>
		/// Destroys and unloads the given <paramref name="entity"/>
		/// </summary>
		void DestroyEntity(UniqueId entity);
		
		/// <summary>
		/// Creates a new Building of the given <paramref name="gameId"/> type on the  given <paramref name="position"/>
		/// and <paramref name="rotation"/>
		/// Returns the Entity representing the new building
		/// </summary>
		UniqueId CreateBuilding(GameId gameId, Vector3 position, Quaternion rotation);
	}
	
	/// <inheritdoc />
	public class EntityLogic : IEntityLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<UniqueId, EntityData> _gameObjectEntityMap = new Dictionary<UniqueId, EntityData>();
		
		private EntityLogic() {}

		public EntityLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}

		/// <inheritdoc />
		public EntityData GetEntityData(UniqueId entity)
		{
			return _gameObjectEntityMap[entity];
		}

		/// <inheritdoc />
		public void DestroyEntity(UniqueId entity)
		{
			_gameObjectEntityMap.Remove(entity);
		}

		/// <inheritdoc />
		public UniqueId CreateBuilding(GameId gameId, Vector3 position, Quaternion rotation)
		{
			if (!gameId.IsInGroup(GameIdGroup.Building))
			{
				throw new ArgumentException($"The game id {gameId} is not a building type game id");
			}
			
			var entity = CreateEntity(gameId);
			
			// TODO: Building Data & Logic

			return entity;
		}

		private UniqueId CreateEntity(GameId gameId)
		{
			var id = _gameLogic.DataProviderLogic.AppData.IdCounter++;
			
			_gameObjectEntityMap.Add(id, new EntityData { Id = id, GameId = gameId });
			
			return id;
		}
	}
}