using GameLovers.Services;

namespace Events
{
	public struct SeedsCollectedEvent : IMessage
	{
		public int OldValue;
		public int NewValue;

		public int Amount => NewValue - OldValue;
	}
}