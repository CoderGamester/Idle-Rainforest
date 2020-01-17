using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct AutomateBuildingCommand : IGameCommand
	{
		public UniqueId BuildingId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Automate(BuildingId);
		}
	}
}