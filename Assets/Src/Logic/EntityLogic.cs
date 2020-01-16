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
		/// Creates a new Building of the given <paramref name="gameId"/> type on the  given <paramref name="position"/>
		/// Returns the Entity representing the new building
		/// </summary>
		UniqueId CreateBuilding(GameId gameId, Vector3 position);
	}
	
	/// <inheritdoc />
	public class EntityLogic : IEntityLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDataProvider _dataProvider;
		
		private EntityLogic() {}

		public EntityLogic(IGameInternalLogic gameLogic, IDataProvider dataProvider)
		{
			_gameLogic = gameLogic;
			_dataProvider = dataProvider;
		}

		/// <inheritdoc />
		public UniqueId CreateBuilding(GameId gameId, Vector3 position)
		{
			if (!gameId.IsInGroup(GameIdGroup.Building))
			{
				throw new ArgumentException($"The game id {gameId} is not a building type game id");
			}
			
			var uniqueId = _dataProvider.GetData<AppData>().UniqueIdCounter++;
			
			CreateGameIdData(uniqueId, gameId);
			CreateBuildingData(uniqueId, position);

			return uniqueId;
		}

		private void CreateGameIdData(UniqueId uniqueId, GameId gameId)
		{
			_dataProvider.GetData<PlayerData>().GameIds.Add(new GameIdData
			{
				Id = uniqueId,
				GameId = gameId
			});
		}

		private void CreateBuildingData(UniqueId uniqueId, Vector3 position)
		{
			_dataProvider.GetData<PlayerData>().Buildings.Add(new BuildingData
			{
				Id = uniqueId,
				Position = position,
				Level = 1,
				ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow,
				IsAutomated = false
			});
		}
	}
}