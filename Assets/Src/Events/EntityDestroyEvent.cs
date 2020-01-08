using GameLovers.Services;
using Ids;

namespace Events
{
	public struct EntityDestroyEvent : IMessage
	{
		public EntityId Entity;
	}
}