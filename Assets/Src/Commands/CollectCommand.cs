using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CollectCommand : IGameCommand
	{
		public UniqueId BuildingId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Collect(BuildingId);
		}
	}
}