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
	public class UpgradeAnimalAchievement : Achievement
	{
		public UpgradeAnimalAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
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
			
			data.CurrentValue++;

			SetData(data);
		}
	}
}