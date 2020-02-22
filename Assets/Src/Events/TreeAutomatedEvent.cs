using GameLovers.Services;
using Ids;

namespace Events
{
	public struct TreeAutomatedEvent : IMessage
	{
		public GameId Tree;
	}
}