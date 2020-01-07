using GameLovers.Services;

namespace Events
{
	public struct ApplicationPauseEvent : IMessage
	{
		public bool IsPaused;
	}
}