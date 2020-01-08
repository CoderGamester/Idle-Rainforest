namespace Logic
{
	public interface IBuildingDataProvider
	{
		
	}

	public interface IBuildingLogic : IBuildingDataProvider
	{
		
	}
	
	/// <inheritdoc />
	public class BuildingLogic : IBuildingLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		
		private BuildingLogic() {}

		public BuildingLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}
	}
}