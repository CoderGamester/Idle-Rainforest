using GameLovers.Services;
using Ids;

namespace Events
{
	public struct CardUpgradedEvent : IMessage
	{
		public GameId Card;
		public int NewLevel;
	}
}