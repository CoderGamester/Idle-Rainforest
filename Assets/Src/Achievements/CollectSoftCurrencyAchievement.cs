using System;
using Data;
using Events;
using Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CollectSoftCurrencyAchievement : Achievement
	{
		public CollectSoftCurrencyAchievement(IGameServices services, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(services, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			Services.MessageBrokerService.Subscribe<SoftCurrencyValueChangedEvent>(OnSoftCurrencyValueChanged);
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