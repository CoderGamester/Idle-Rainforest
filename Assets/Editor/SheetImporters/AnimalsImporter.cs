using Configs;
using GameLoversEditor.GoogleSheetImporter;

namespace SheetImporters
{
	/// <inheritdoc />
	public class AnimalsImporter : GoogleSheetConfigsImporter<AnimalConfig, AnimalConfigs>
	{
		/// <inheritdoc />
		public override string GoogleSheetUrl => "https://docs.google.com/spreadsheets/d/1CFp3P0262Bn_EfYSTTLwzbeT5KsyzgJx5GJfRKbNEm4/edit#gid=1472935835";
	}
}