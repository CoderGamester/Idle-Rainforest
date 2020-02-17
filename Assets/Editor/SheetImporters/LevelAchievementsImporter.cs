using System.Collections.Generic;
using Achievements;
using Configs;
using Data;
using GameLovers.GoogleSheetImporter;
using GameLoversEditor.GoogleSheetImporter;
using Ids;

namespace SheetImporters
{
	/// <inheritdoc />
	public class LevelAchievementsImporter : GoogleSheetConfigsImporter<LevelAchievementConfig, LevelAchievementConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1CFp3P0262Bn_EfYSTTLwzbeT5KsyzgJx5GJfRKbNEm4/edit#gid=938448025";
		
		/// <inheritdoc />
		protected override LevelAchievementConfig Deserialize(Dictionary<string, string> data)
		{
			const int maxAchievements = 11;
			
			var config = CsvParser.DeserializeTo<LevelAchievementConfig>(data);
			
			config.Achievements = new List<AchievementConfig>();

			for (var i = 1; i <= maxAchievements; i++)
			{
				config.Achievements.Add(new AchievementConfig
				{
					Type = CsvParser.Parse<AchievementType>(data[$"Type{i.ToString()}"]),
					RequirementAmount = CsvParser.Parse<int>(data[$"Requirement{i.ToString()}"]),
					RewardAmount = CsvParser.Parse<int>(data[$"Reward{i.ToString()}"]),
				});
			}

			return config;
		}
	}
}