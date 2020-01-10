using Events;
using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CollectSeedsCommand : IGameCommand
	{
		public EntityId Entity;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Collect(Entity);
		}
	}
}