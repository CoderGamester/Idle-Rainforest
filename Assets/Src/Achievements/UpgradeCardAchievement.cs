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
	public class UpgradeCardAchievement : Achievement
	{
		public UpgradeCardAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
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
			var data = Data;

			if (data.Goal.GameId != GameId.Random && eventData.Card != data.Goal.GameId)
			{
				return;
			}
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}