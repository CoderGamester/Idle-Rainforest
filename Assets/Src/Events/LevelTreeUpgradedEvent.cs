using GameLovers.Services;
using Ids;

namespace Events
{
	public struct LevelTreeUpgradedEvent : IMessage
	{
		public GameId Tree;
		public int NewLevel;
	}
}