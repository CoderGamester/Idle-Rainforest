using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct UpgradeBuildingCommand : IGameCommand
	{
		public EntityId Entity;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Upgrade(Entity);
		}
	}
}