using GameLovers.Services;

namespace Events
{
	public struct SeedsValueChangedEvent : IMessage
	{
		public int OldValue;
		public int NewValue;

		public int Amount => NewValue - OldValue;
	}
}