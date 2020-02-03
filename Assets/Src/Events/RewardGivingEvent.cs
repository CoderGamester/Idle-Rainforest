using Data;
using GameLovers.Services;

namespace Events
{
	public struct RewardGivingEvent : IMessage
	{
		public IntData Reward;
	}
}