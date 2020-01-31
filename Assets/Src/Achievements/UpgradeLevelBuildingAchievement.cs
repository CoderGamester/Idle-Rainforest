using System;
using Data;
using Events;
using GameLovers.Services;
using Ids;
using Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class UpgradeLevelBuildingAchievement : Achievement
	{
		public UpgradeLevelBuildingAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<LevelBuildingUpgradedEvent>(OnLevelBuildingUpgraded);
		}

		private void OnLevelBuildingUpgraded(LevelBuildingUpgradedEvent eventData)
		{
			var data = Data;

			if (data.Goal.GameId != GameId.Random && eventData.Building != data.Goal.GameId)
			{
				return;
			}
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}