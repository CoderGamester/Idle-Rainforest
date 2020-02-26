using System.Collections.Generic;
using Configs;
using Data;
using GameLovers.GoogleSheetImporter;
using GameLoversEditor.GoogleSheetImporter;
using Ids;

namespace SheetImporters
{
	/// <inheritdoc />
	public class LevelTreeImporter : GoogleSheetConfigsImporter<LevelTreeConfig, LevelTreeConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1CFp3P0262Bn_EfYSTTLwzbeT5KsyzgJx5GJfRKbNEm4/edit#gid=880604694";
	}
}