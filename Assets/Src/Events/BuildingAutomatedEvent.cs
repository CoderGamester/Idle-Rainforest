using GameLovers.Services;
using Ids;

namespace Events
{
	public struct BuildingAutomatedEvent : IMessage
	{
		public GameId Building;
	}
}