using System;
using Data;
using Events;
using GameLovers.Services;
using Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CollectSoftCurrencyAchievement : Achievement
	{
		public CollectSoftCurrencyAchievement(IMessageBrokerService messageBroker, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
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
			if (eventData.Amount < 0)
			{
				return;
			}
			
			var data = Data;
			
			data.CurrentValue += eventData.Amount;

			SetData(data);
		}
	}
}