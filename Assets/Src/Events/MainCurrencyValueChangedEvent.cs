using GameLovers.Services;
using Ids;

namespace Events
{
	public struct MainCurrencyValueChangedEvent : IMessage
	{
		public int OldValue;
		public int NewValue;

		public int Amount => NewValue - OldValue;
	}
}