using GameLovers.Services;
using Ids;

namespace Events
{
	public struct AchievementCollectedEvent : IMessage
	{
		public UniqueId Id;
	}
}