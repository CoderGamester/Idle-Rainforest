using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct UpgradeBuildingCommand : IGameCommand
	{
		public UniqueId UniqueId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Upgrade(UniqueId);
		}
	}
}