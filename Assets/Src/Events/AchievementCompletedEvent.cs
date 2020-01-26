using GameLovers.Services;
using Ids;

namespace Events
{
	public struct AchievementCompletedEvent : IMessage
	{
		public UniqueId Id;
	}
}