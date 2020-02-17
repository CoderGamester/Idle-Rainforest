using GameLovers.Services;
using Ids;

namespace Events
{
	public struct CardCollectedEvent : IMessage
	{
		public GameId Card;
		public int Amount;
	}
}