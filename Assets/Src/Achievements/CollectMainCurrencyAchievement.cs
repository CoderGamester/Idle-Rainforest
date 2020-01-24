using System;
using Data;
using Events;
using Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class CollectMainCurrencyAchievement : Achievement
	{
		public CollectMainCurrencyAchievement(IGameServices services, Func<AchievementData> achievementResolver, Action<AchievementData> setter) :
			base(services, achievementResolver, setter)
		{
		}
		
		/// <inheritdoc />
		protected override void SubscribeMessages()
		{
			Services.MessageBrokerService.Subscribe<MainCurrencyValueChangedEvent>(OnMainCurrencyValueChanged);
		}

		private void OnMainCurrencyValueChanged(MainCurrencyValueChangedEvent eventData)
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