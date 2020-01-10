using GameLovers.Services;

namespace Events
{
	public struct MainCurrencyValueChangedEvent : IMessage
	{
		public float OldValue;
		public float NewValue;

		public float Amount => NewValue - OldValue;
	}
}