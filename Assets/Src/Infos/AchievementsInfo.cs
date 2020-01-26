using System.Collections.Generic;
using Data;

namespace Infos
{
	public struct AchievementsInfo
	{
		public int Completed;
		public int Collected;
		public IReadOnlyList<AchievementData> Achievements;

		public int Total => Achievements.Count;
	}
}