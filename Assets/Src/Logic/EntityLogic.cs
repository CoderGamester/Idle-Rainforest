using System;
using System.Collections.Generic;
using Events;
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
		/// Requests the <see cref="GameId"/> of the given <paramref name="entity"/>
		/// </summary>
		GameId GetGameId(EntityId entity);
	}
	
	/// <inheritdoc />
	public interface IEntityLogic : IEntityDataProvider
	{
		/// <summary>
		/// Destroys and unloads the given <paramref name="entity"/>
		/// </summary>
		void DestroyEntity(EntityId entity);
		
		/// <summary>
		/// Creates a new Building of the given <paramref name="gameId"/> type on the  given <paramref name="position"/>
		/// Returns the Entity representing the new building
		/// </summary>
		EntityId CreateBuilding(GameId gameId, Vector3 position);
	}
	
	/// <inheritdoc />
	public class EntityLogic : IEntityLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<EntityId, GameId> _gameIdMap = new Dictionary<EntityId, GameId>();
		
		private EntityLogic() {}

		public EntityLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}

		/// <inheritdoc />
		public GameId GetGameId(EntityId entity)
		{
			return _gameIdMap[entity];
		}

		/// <inheritdoc />
		public void DestroyEntity(EntityId entity)
		{
			_gameIdMap.Remove(entity);
			_gameLogic.MessageBrokerService.Publish(new EntityDestroyEvent { Entity = entity });
		}

		/// <inheritdoc />
		public EntityId CreateBuilding(GameId gameId, Vector3 position)
		{
			if (!gameId.IsInGroup(GameIdGroup.Building))
			{
				throw new ArgumentException($"The game id {gameId} is not a building type game id");
			}
			
			var entity = CreateEntity(gameId);
			
			// TODO: Building Data & Logic

			return entity;
		}

		private EntityId CreateEntity(GameId gameId)
		{
			var id = _gameLogic.DataProviderLogic.AppData.IdCounter++;
			
			_gameIdMap.Add(id, gameId);
			
			return id;
		}
	}
}