using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct AchievementData
	{
		public UniqueId Id;
		public GameId AchievementType;
		public int CurrentValue;
		public IntData Goal;
		public IntData Reward;
		public bool IsCollected;

		public bool IsCompleted => CurrentValue == Goal.Value;
	}
}