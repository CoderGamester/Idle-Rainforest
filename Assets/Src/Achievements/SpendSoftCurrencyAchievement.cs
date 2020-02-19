using System;
using Data;
using Events;
using GameLovers.Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class SpendSoftCurrencyAchievement : Achievement
	{
		public SpendSoftCurrencyAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(messageBroker, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			MessageBroker.Subscribe<SoftCurrencyValueChangedEvent>(OnSoftCurrencyValueChanged);
		}

		private void OnSoftCurrencyValueChanged(SoftCurrencyValueChangedEvent eventData)
		{
			if (eventData.Amount > 0)
			{
				return;
			}
			
			var data = Data;
			
			data.CurrentValue += Math.Abs(eventData.Amount);

			SetData(data);
		}
	}
}