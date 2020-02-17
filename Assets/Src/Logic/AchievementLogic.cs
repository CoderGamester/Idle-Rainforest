using System;
using System.Collections.Generic;
using Achievements;
using Configs;
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
		/// TODO: REMOVE
		/// </summary>
		new IUniqueIdList<AchievementData> Data { get; }

		/// <summary>
		/// TODO:
		/// </summary>
		void CollectAchievement(UniqueId id);
	}
	
	/// <inheritdoc />
	public class AchievementLogic : IAchievementLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IUniqueIdList<AchievementData> _data;
		private readonly IList<Achievement> _runningAchievements = new List<Achievement>();

		/// <inheritdoc />
		public IUniqueIdListReader<AchievementData> Data => _data;

		/// <inheritdoc />
		IUniqueIdList<AchievementData> IAchievementLogic.Data => _data;

		private AchievementLogic() {}

		public AchievementLogic(IGameInternalLogic gameLogic, LevelData levelData)
		{
			_gameLogic = gameLogic;
			_data = new UniqueIdList<AchievementData>(x => x.Id, levelData.Achievements);
		}

		/// <summary>
		/// Initializes the AchievementLogic to create the achievements and set their states in the game
		/// </summary>
		public void Init()
		{
			var list = _data.GetList();
			var animals = GameIdGroup.Animal.GetIds();

			if (list.Count == 0)
			{
				var config = _gameLogic.ConfigsProvider.GetConfig<LevelAchievementConfig>(1);

				foreach (var achievement in config.Achievements)
				{
					var reward = new IntData(animals[Random.Range(0, animals.Count)], achievement.RewardAmount);
					
					_gameLogic.EntityLogic.CreateAchievement(achievement.Type, achievement.RequirementAmount, reward);
				}
			}
			
			for (var i = 0; i < list.Count; i++)
			{
				var indexClosure = i;
				
				_runningAchievements.Add(AchievementFactory(list[i].AchievementType, () => list[indexClosure]));
			}
		}

		/// <inheritdoc />
		public AchievementsInfo GetInfo()
		{
			var list = _data.GetList() as List<AchievementData>;
			var completed = 0;
			var collected = 0;
			
			for (int i = 0; i < list.Count; i++)
			{
				completed += list[i].IsCompleted ? 1 : 0;
				collected += list[i].IsCollected ? 1 : 0;
			}
			
			list.Sort((elem1, elem2) => elem1.Id.CompareTo(elem2.Id));
			
			return new AchievementsInfo
			{
				Completed = completed,
				Collected = collected,
				Achievements = list
			};
		}

		/// <inheritdoc />
		public void CollectAchievement(UniqueId id)
		{
			var data = _data.Get(id);

			if (data.IsCollected)
			{
				throw new LogicException($"Cannot collect the {data.AchievementType} achievement because is already collected");
			}

			data.IsCollected = true;
			
			_data.Set(data);
			_gameLogic.RewardLogic.GiveReward(data.Reward);
			_gameLogic.MessageBrokerService.Publish(new AchievementCollectedEvent { Id = id });
		}

		private Achievement AchievementFactory(AchievementType achievementType, Func<AchievementData> resolver)
		{
			switch (achievementType)
			{
				case AchievementType.SpendMainCurrency:
					return new SpendMainCurrencyAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.CollectMainCurrency:
					return new CollectMainCurrencyAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.SpendSoftCurrency:
					return new SpendSoftCurrencyAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.CollectSoftCurrency:
					return new CollectSoftCurrencyAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.CollectCards:
					return new CollectCardsAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.UpgradeAnimal:
					return new UpgradeAnimalAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.UpgradeTree:
					throw new LogicException($"Not implemented yet");
				case AchievementType.UpgradeLevelTree:
					return new UpgradeLevelTreeAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.RankUpTree:
					return new RankUpTreeAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				case AchievementType.AutomateTree:
					return new AutomateAchievement(_gameLogic.MessageBrokerService, resolver, _data.Set);
				default:
					throw new LogicException($"Missing achievementType {achievementType}");
			}
		}
	}
}