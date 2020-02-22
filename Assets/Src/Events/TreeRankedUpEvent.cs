using GameLovers.Services;
using Ids;

namespace Events
{
	public struct TreeRankedUpEvent : IMessage
	{
		public GameId Tree;
		public int Level;
	}
}