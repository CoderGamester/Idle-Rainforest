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
	public class UpgradeLevelTreeAchievement : Achievement
	{
		public UpgradeLevelTreeAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<LevelTreeUpgradedEvent>(OnLevelBuildingUpgraded);
		}

		private void OnLevelBuildingUpgraded(LevelTreeUpgradedEvent eventData)
		{
			var data = Data;

			if (data.Goal.GameId != GameId.Random && eventData.Tree != data.Goal.GameId)
			{
				return;
			}
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}