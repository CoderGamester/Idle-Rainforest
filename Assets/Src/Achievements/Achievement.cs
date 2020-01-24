using System;
using Data;
using Events;
using Services;

namespace Achievements
{
	/// <summary>
	/// TODO:
	/// </summary>
	public abstract class Achievement
	{
		protected readonly IGameServices Services;
		
		private readonly Func<AchievementData> _achievementResolver;
		private readonly Action<AchievementData> _setter;

		/// <summary>
		/// TODO:
		/// </summary>
		public AchievementData Data => _achievementResolver();
		
		protected Achievement(IGameServices services, Func<AchievementData> achievementResolver, Action<AchievementData> setter)
		{
			Services = services;
			_achievementResolver = achievementResolver;
			_setter = setter;

			if (!Data.IsCompleted)
			{
				// ReSharper disable once VirtualMemberCallInConstructor
				SubscribeMessages();
			}
		}

		/// <summary>
		/// TODO:
		/// </summary>
		public void Disable()
		{
			Services.MessageBrokerService.UnsubscribeAll(this);

			OnDisable();
		}

		/// <summary>
		/// Subscribes to messages from the <see cref="GameLovers.Services.MessageBrokerService"/>.
		/// IMPORTANT: Shouldn't do more than subscribing to messages because of this abstract methods behind executed before the derived constructor.
		/// </summary>
		protected abstract void SubscribeMessages();
		
		
		protected virtual void OnDisable() { }

		protected void SetData(AchievementData data)
		{
			data.CurrentValue = data.CurrentValue > data.Goal.Value ? data.Goal.Value : data.CurrentValue;
			
			_setter(data);

			if (data.IsCompleted)
			{
				Services.MessageBrokerService.Publish(new AchievementCompletedEvent { Id = data.Id } );
				
				Disable();
			}
		}
	}
}