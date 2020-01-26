using System;
using System.Collections.Generic;
using Data;
using Events;
using Ids;
using Infos;
using Random = UnityEngine.Random;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IAchievementDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		IUniqueIdListReader<AchievementData> Data { get; }

		/// <summary>
		/// TODO:
		/// </summary>
		AchievementsInfo GetInfo();
	}

	/// <inheritdoc />
	public interface IAchievementLogic : IAchievementDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		UniqueId GenerateRandomAchievement();

		/// <summary>
		/// TODO:
		/// </summary>
		void Collect(UniqueId id);
	}
	
	/// <inheritdoc />
	public class AchievementLogic : IAchievementLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IUniqueIdList<AchievementData> _data;

		/// <inheritdoc />
		public IUniqueIdListReader<AchievementData> Data => _data;

		private AchievementLogic() {}

		public AchievementLogic(IGameInternalLogic gameLogic, LevelData levelData)
		{
			_gameLogic = gameLogic;
			_data = new UniqueIdList<AchievementData>(x => x.Id, levelData.Achievements);
		}

		/// <inheritdoc />
		public AchievementsInfo GetInfo()
		{
			var list = _data.GetList();
			var completed = 0;
			var collected = 0;
			
			for (int i = 0; i < list.Count; i++)
			{
				completed += list[i].IsCompleted ? 1 : 0;
				collected += list[i].IsCollected ? 1 : 0;
			}
			
			return new AchievementsInfo
			{
				Completed = completed,
				Collected = collected,
				Achievements = list as IReadOnlyList<AchievementData>
			};
		}

		/// <inheritdoc />
		public UniqueId GenerateRandomAchievement()
		{
			var achievements = GameIdGroup.Achievement.GetIds();
			var cards = GameIdGroup.Card.GetIds();
			var type = achievements[Random.Range(0, achievements.Count)];
			var goal = new IntData();
			var reward = new IntData
			{
				GameId = cards[Random.Range(0, cards.Count)],
				Value = Random.Range(3, 6)
			};

			switch (type)
			{
				case GameId.CollectMainCurrency:
					goal.GameId = GameId.MainCurrency;
					goal.Value = Random.Range(1000, 2001);
					break;
				case GameId.CollectSoftCurrency:
					goal.GameId = GameId.SoftCurrency;
					goal.Value = Random.Range(1000, 2001);
					break;
				case GameId.UpgradeCard:
					goal.GameId = GameId.Random;
					goal.Value = Random.Range(2, 5);
					break;
				case GameId.UpgradeLevelBuilding:
					goal.GameId = GameId.Random;
					goal.Value = Random.Range(2, 5);
					break;
				case GameId.AutomateBuilding:
					goal.GameId = GameId.Random;
					goal.Value = Random.Range(1, 5);
					break;
				default:
					throw new ArgumentOutOfRangeException($"The given id {type} is no of {nameof(GameIdGroup.Achievement)} group type");
			}

			return _gameLogic.EntityLogic.CreateAchievement(type, goal, reward);
		}

		/// <inheritdoc />
		public void Collect(UniqueId id)
		{
			var data = _data.Get(id);

			if (data.IsCollected)
			{
				throw new InvalidOperationException($"Cannot collect the {data.AchievementType} achievement because is already collected");
			}

			data.IsCollected = true;
			
			_data.Set(data);
			_gameLogic.RewardLogic.GiveReward(data.Reward);
			_gameLogic.MessageBrokerService.Publish(new AchievementCollectedEvent { Id = id });
		}
	}
}