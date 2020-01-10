using GameLovers.Services;
using Ids;

namespace Events
{
	public struct BuildingUpgradedEvent : IMessage
	{
		public EntityId Entity;
	}
}