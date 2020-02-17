using System;
using Data;
using Events;
using GameLovers.Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class RankUpTreeAchievement : Achievement
	{
		public RankUpTreeAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<TreeRankedUpEvent>(OnTreeRankedUp);
		}

		private void OnTreeRankedUp(TreeRankedUpEvent eventData)
		{
			var data = Data;
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}