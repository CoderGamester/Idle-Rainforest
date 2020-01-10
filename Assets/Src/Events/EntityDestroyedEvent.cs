using GameLovers.Services;
using Ids;

namespace Events
{
	public struct EntityDestroyedEvent : IMessage
	{
		public EntityId Entity;
	}
}