using System;
using Achievements;
using Ids;

namespace Data
{
	[Serializable]
	public struct AchievementData
	{
		public UniqueId Id;
		public AchievementType AchievementType;
		public int CurrentValue;
		public int Goal;
		public IntData Reward;
		public bool IsCollected;

		public bool IsCompleted => CurrentValue >= Goal;
	}
}