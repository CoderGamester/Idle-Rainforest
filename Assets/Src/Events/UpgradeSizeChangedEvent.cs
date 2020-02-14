using GameLovers.Services;

namespace Events
{
	public struct UpgradeSizeChangedEvent : IMessage
	{
		public uint UpgradeSize;

		public bool IsMaxSize => UpgradeSize == 0;
	}
}