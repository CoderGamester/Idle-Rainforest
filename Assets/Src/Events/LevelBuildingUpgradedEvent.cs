using GameLovers.Services;
using Ids;

namespace Events
{
	public struct LevelBuildingUpgradedEvent : IMessage
	{
		public GameId Building;
		public int NewLevel;
	}
}