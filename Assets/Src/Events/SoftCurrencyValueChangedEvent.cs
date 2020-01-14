using GameLovers.Services;

namespace Events
{
	public struct SoftCurrencyValueChangedEvent : IMessage
	{
		public int OldValue;
		public int NewValue;

		public int Amount => NewValue - OldValue;
	}
}