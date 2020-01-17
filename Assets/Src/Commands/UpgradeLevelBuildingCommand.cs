using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct UpgradeLevelBuildingCommand : IGameCommand
	{
		public UniqueId BuildingId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Upgrade(BuildingId);
		}
	}
}