using System;
using Data;
using Events;
using GameLovers.Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CollectCardsAchievement : Achievement
	{
		public CollectCardsAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<CardCollectedEvent>(OnCardCollected);
		}

		private void OnCardCollected(CardCollectedEvent eventData)
		{
			var data = Data;
			
			data.CurrentValue += eventData.Amount;

			SetData(data);
		}
	}
}