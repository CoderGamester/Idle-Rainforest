using System;
using Data;
using Events;
using Ids;
using Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class AutomateAchievement : Achievement
	{
		public unsafe AutomateAchievement(IGameServices services, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(services, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			Services.MessageBrokerService.Subscribe<BuildingAutomatedEvent>(OnBuildingAutomated);
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