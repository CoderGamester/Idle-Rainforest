using System;
using Achievements;
using Data;
using Events;
using Ids;
using UnityEngine;
using Random = UnityEngine.Random;

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
		/// Creates a new Tree of the given <paramref name="gameId"/> type on the  given <paramref name="position"/>
		/// and returns the <see cref="UniqueId"/> representing the newly created entity
		/// </summary>
		UniqueId CreateTree(GameId gameId, Vector3 position);

		/// <summary>
		/// TODO:
		/// </summary>
		UniqueId CreateAchievement(AchievementType achievementType, int goal, IntData reward);
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
		public UniqueId CreateTree(GameId gameId, Vector3 position)
		{
			if (!gameId.IsInGroup(GameIdGroup.Tree))
			{
				throw new LogicException($"The game id {gameId} is not a building type game id");
			}
			
			var uniqueId = _dataProvider.AppData.UniqueIdCounter++;
			
			CreateGameIdData(uniqueId, gameId);
			CreateTreeData(uniqueId, position);

			return uniqueId;
		}

		/// <inheritdoc />
		public UniqueId CreateAchievement(AchievementType achievementType, int goal, IntData reward)
		{
			var uniqueId = _dataProvider.AppData.UniqueIdCounter++;
			
			CreateAchievementData(uniqueId, achievementType, goal, reward);

			return uniqueId;
		}

		private void CreateGameIdData(UniqueId uniqueId, GameId gameId)
		{
			_dataProvider.PlayerData.GameIds.Add(new GameIdData
			{
				Id = uniqueId,
				GameId = gameId
			});
		}

		private void CreateTreeData(UniqueId uniqueId, Vector3 position)
		{
			_dataProvider.LevelData.Buildings.Add(new LevelTreeData
			{
				Id = uniqueId,
				Position = position,
				Level = 0,
				ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow,
				IsAutomated = false
			});
		}

		private void CreateAchievementData(UniqueId uniqueId, AchievementType achievementType, int goal, IntData reward)
		{
			_dataProvider.LevelData.Achievements.Add(new AchievementData
			{
				Id = uniqueId,
				AchievementType = achievementType,
				CurrentValue = 0,
				Goal = goal,
				Reward = reward
			});
		}
	}
}