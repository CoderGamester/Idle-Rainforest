using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	/// <summary>
	/// Contains all the data in the scope of the Level 
	/// </summary>
	[Serializable]
	public class LevelData
	{
		public readonly List<LevelTreeData> Buildings = new List<LevelTreeData>();
		public readonly List<AchievementData> Achievements = new List<AchievementData>();
	}
}