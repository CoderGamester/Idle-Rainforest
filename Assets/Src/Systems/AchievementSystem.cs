using System;
using System.Collections.Generic;
using Achievements;
using Data;
using GameLovers.Services;
using Ids;
using Services;

namespace Systems
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class AchievementSystem
	{
		private readonly IGameServices _services;
		private readonly IUniqueIdList<AchievementData> _achievements;
		private readonly IList<Achievement> _runningAchievements = new List<Achievement>();
		
		private AchievementSystem() {}

		public AchievementSystem(IList<AchievementData> achievements)
		{
			_services = MainInstaller.Resolve<IGameServices>();
			_achievements = new UniqueIdList<AchievementData>(x => x.Id, achievements);

			CreateAchievements();
		}

		private void CreateAchievements()
		{
			var list = _achievements.GetList();
			
			for (var i = 0; i < list.Count; i++)
			{
				var index = i;
				Func<AchievementData> resolver = () => list[index];
				Action<AchievementData> setter = data => _achievements.Set(data);
				
				switch (list[i].AchievementType)
				{
					case GameId.CollectMainCurrency:
						_runningAchievements.Add(new CollectMainCurrencyAchievement(_services, resolver, setter));
						break;
					case GameId.CollectSoftCurrency:
						_runningAchievements.Add(new CollectSoftCurrencyAchievement(_services, resolver, setter));
						break;
					case GameId.UpgradeCard:
						_runningAchievements.Add(new UpgradeCardAchievement(_services, resolver, setter));
						break;
					case GameId.UpgradeLevelBuilding:
						_runningAchievements.Add(new UpgradeLevelBuildingAchievement(_services, resolver, setter));
						break;
					case GameId.AutomateBuilding:
						_runningAchievements.Add(new AutomateAchievement(_services, resolver, setter));
						break;
					default:
						throw new ArgumentOutOfRangeException($"The given id {list[i].AchievementType} is not " +
						                                      $"of {nameof(GameIdGroup.Achievement)} group type");
				}
			}
		}
	}
}