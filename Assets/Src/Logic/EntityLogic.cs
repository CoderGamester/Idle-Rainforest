using System;
using Data;
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
		private readonly ISessionDataProvider _sessionDataProvider;
		
		private EntityLogic() {}

		public EntityLogic(IGameInternalLogic gameLogic, ISessionDataProvider sessionDataProvider)
		{
			_gameLogic = gameLogic;
			_sessionDataProvider = sessionDataProvider;
		}

		/// <inheritdoc />
		public void DestroyEntity(EntityId entity)
		{ 
			foreach (var data in _sessionDataProvider.SessionData)
			{
				data.Value.RemoveEntity(entity);
			}

			if (_gameLogic.GameObjectLogic.HasGameObject(entity))
			{
				_gameLogic.GameObjectLogic.UnloadGameObject(entity);
			}
			
			_gameLogic.MessageBrokerService.Publish(new EntityDestroyedEvent { Entity = entity });
		}

		/// <inheritdoc />
		public EntityId CreateBuilding(GameId gameId, Vector3 position)
		{
			if (!gameId.IsInGroup(GameIdGroup.Building))
			{
				throw new ArgumentException($"The game id {gameId} is not a building type game id");
			}
			
			var entity = _sessionDataProvider.EntityCounter++;
			
			CreateGameIdData(entity, gameId);
			CreateBuildingData(entity, gameId, position);

			return entity;
		}

		private void CreateGameIdData(EntityId entity, GameId gameId)
		{
			_sessionDataProvider.GetSessionData<GameId>().Add(entity, gameId);
		}

		private void CreateBuildingData(EntityId entity, GameId gameId, Vector3 position)
		{
			_sessionDataProvider.GetSessionData<int>().Add(entity, _gameLogic.DataProviderLogic.PlayerData.Buildings.Count);
			
			_gameLogic.DataProviderLogic.PlayerData.Buildings.Add(new BuildingData
			{
				GameId = gameId,
				Position = position,
				Level = 0,
				ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow
			});
		}
	}
}