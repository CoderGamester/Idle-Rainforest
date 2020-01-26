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
	public class UpgradeCardAchievement : Achievement
	{
		public UpgradeCardAchievement(IGameServices services, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(services, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			Services.MessageBrokerService.Subscribe<CardUpgradedEvent>(OnCardUpgraded);
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