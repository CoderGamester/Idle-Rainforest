using System.Collections.Generic;
using Data;
using Ids;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IBuildingDataProvider
	{
		
	}

	/// <inheritdoc />
	public interface IBuildingLogic : IBuildingDataProvider
	{
		
	}
	
	/// <inheritdoc />
	public class BuildingLogic : IBuildingLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<EntityId, BuildingData> _data;
		
		private BuildingLogic() {}

		public BuildingLogic(IGameInternalLogic gameLogic, IDictionary<EntityId, BuildingData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}
	}
}