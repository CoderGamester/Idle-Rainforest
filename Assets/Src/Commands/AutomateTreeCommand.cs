using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct AutomateTreeCommand : IGameCommand
	{
		public UniqueId TreeId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Automate(TreeId);
		}
	}
}