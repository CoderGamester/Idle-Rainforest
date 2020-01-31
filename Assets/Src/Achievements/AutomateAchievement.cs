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
	public class AutomateAchievement : Achievement
	{
		public unsafe AutomateAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<BuildingAutomatedEvent>(OnBuildingAutomated);
		}

		private void OnBuildingAutomated(BuildingAutomatedEvent eventData)
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