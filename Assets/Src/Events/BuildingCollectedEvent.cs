using GameLovers.Services;
using Ids;

namespace Events
{
	public struct BuildingCollectedEvent : IMessage
	{
		public GameId Building;
	}
}