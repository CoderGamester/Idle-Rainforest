using System;
using Data;
using Events;
using GameLovers.Services;
using Ids;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class UpgradeTreeAchievement : Achievement
	{
		public UpgradeTreeAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<CardUpgradedEvent>(OnCardUpgraded);
		}

		private void OnCardUpgraded(CardUpgradedEvent eventData)
		{
			if (!eventData.Card.IsInGroup(GameIdGroup.Tree))
			{
				return;
			}
			
			var data = Data;
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}