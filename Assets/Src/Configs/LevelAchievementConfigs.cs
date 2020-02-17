using System;
using System.Collections.Generic;
using Achievements;
using GameLovers.ConfigsContainer;
using GameLovers.GoogleSheetImporter;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct AchievementConfig
	{
		public AchievementType Type;
		public int RequirementAmount;
		public int RewardAmount;
	}
	
	[Serializable]
	public struct LevelAchievementConfig
	{
		public int Level;
		public int CompleteRequirement;
		[ParseIgnore] public List<AchievementConfig> Achievements;
	}
	
	/// <summary>
	/// Scriptable Object tool to import the <seealso cref="LevelAchievementConfig"/> sheet data
	/// </summary>
	[CreateAssetMenu(fileName = "LevelAchievementConfigs", menuName = "ScriptableObjects/Configs/LevelAchievementConfigs")]
	public class LevelAchievementConfigs : ScriptableObject, IConfigsContainer<LevelAchievementConfig>
	{
		[SerializeField]
		private List<LevelAchievementConfig> _configs = new List<LevelAchievementConfig>();

		// ReSharper disable once ConvertToAutoProperty
		public List<LevelAchievementConfig> Configs => _configs;
	}
}