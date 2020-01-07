using GameConfigs;
using GameLoversEditor.GoogleSheetImporter;

namespace SheetImporters
{
	/// <inheritdoc />
	public class BuildingsImporter : GoogleSheetConfigsImporter<BuildingConfig, BuildingConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1pxV7Fp8T9ea-Bp1ts0kn0JwAi1M3RyLpibq4LyOYQT8/edit#gid=0";
	}
}