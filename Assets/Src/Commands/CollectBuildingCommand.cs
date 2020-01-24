using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CollectBuildingCommand : IGameCommand
	{
		public UniqueId BuildingId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Collect(BuildingId);
		}
	}
}