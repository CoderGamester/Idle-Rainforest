using GameLovers.Services;
using Ids;

namespace Events
{
	public struct CurrencyValueChangedEvent : IMessage
	{
		public GameId CurrencyType;
		public int OldValue;
		public int NewValue;

		public int Amount => NewValue - OldValue;
	}
}