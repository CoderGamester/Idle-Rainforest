using System.Collections.Generic;
using Configs;
using Data;
using GameLovers.GoogleSheetImporter;
using GameLoversEditor.GoogleSheetImporter;
using Ids;

namespace SheetImporters
{
	/// <inheritdoc />
	public class LevelBuildingImporter : GoogleSheetConfigsImporter<LevelTreeConfig, LevelTreeConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1CFp3P0262Bn_EfYSTTLwzbeT5KsyzgJx5GJfRKbNEm4/edit#gid=880604694";
		
		/// <inheritdoc />
		protected override LevelTreeConfig Deserialize(Dictionary<string, string> data)
		{
			var config = CsvParser.DeserializeTo<LevelTreeConfig>(data);
			var arrayRewards = CsvParser.ArrayParse<string>(data[$"{nameof(LevelTreeConfig.UpgradeRewards)}"]);
			var arrayBrackets = CsvParser.ArrayParse<string>(data[$"{nameof(LevelTreeConfig.UpgradeBrackets)}"]);
			var rewards = new List<IntData>();
			var brackets = new List<IntPairData>();
			
			for(var i = 0; i < arrayRewards.Length; i += 2)
			{
				var gameId = CsvParser.Parse<GameId>(arrayRewards[i]);
				var intValue = int.Parse(arrayRewards[i + 1]);
					
				rewards.Add(new IntData(gameId, intValue));
			}
			
			for(var i = 0; i < arrayBrackets.Length; i += 2)
			{
				var intKey = int.Parse(arrayBrackets[i]);
				var intValue = int.Parse(arrayBrackets[i + 1]);
					
				brackets.Add(new IntPairData(intKey, intValue));
			}

			config.UpgradeRewards = rewards;
			config.UpgradeBrackets = brackets;

			return config;
		}
	}
}