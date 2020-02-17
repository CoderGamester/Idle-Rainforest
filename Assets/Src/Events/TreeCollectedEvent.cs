using GameLovers.Services;
using Ids;

namespace Events
{
	public struct TreeCollectedEvent : IMessage
	{
		public GameId Tree;
	}
}