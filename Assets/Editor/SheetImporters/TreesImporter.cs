using Configs;
using GameLoversEditor.GoogleSheetImporter;

namespace SheetImporters
{
	/// <inheritdoc />
	public class TreesImporter : GoogleSheetConfigsImporter<TreeConfig, TreeConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1CFp3P0262Bn_EfYSTTLwzbeT5KsyzgJx5GJfRKbNEm4/edit#gid=254019770";
	}
}