using GameLovers.Services;

namespace Events
{
	public struct HardCurrencyValueChangedEvent : IMessage
	{
		public int OldValue;
		public int NewValue;

		public int Amount => NewValue - OldValue;
	}
}