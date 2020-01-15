using System.Collections.Generic;
using Configs;
using Data;
using GameLovers.GoogleSheetImporter;
using GameLoversEditor.GoogleSheetImporter;
using Ids;

namespace SheetImporters
{
	/// <inheritdoc />
	public class BuildingsImporter : GoogleSheetConfigsImporter<BuildingConfig, BuildingConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1CFp3P0262Bn_EfYSTTLwzbeT5KsyzgJx5GJfRKbNEm4/edit#gid=880604694";
		
		/// <inheritdoc />
		protected override BuildingConfig Deserialize(Dictionary<string, string> data)
		{
			var config = CsvParser.DeserializeTo<BuildingConfig>(data);
			var pair = CsvParser.PairParse<GameId, int>(data[$"{nameof(BuildingConfig.AutomationCardLevelRequired)}"]);
			var array = CsvParser.ArrayParse<string>(data[$"{nameof(BuildingConfig.UpgradeRewards)}"]);
			var rewards = new List<IntData>();
			
			for(var i = 0; i < array.Length; i += 2)
			{
				var gameId = CsvParser.Parse<GameId>(array[i]);
				var intValue = int.Parse(array[i + 1]);
					
				rewards.Add(new IntData(gameId, intValue));
			}

			config.AutomationCardLevelRequired = new IntData(pair.Key, pair.Value);
			config.UpgradeRewards = rewards.AsReadOnly();

			return config;
		}
	}
}