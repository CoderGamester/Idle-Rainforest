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
			MessageBroker.Subscribe<TreeAutomatedEvent>(OnBuildingAutomated);
		}

		private void OnBuildingAutomated(TreeAutomatedEvent eventData)
		{
			var data = Data;
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}