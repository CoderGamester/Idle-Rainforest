using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CollectTreeCommand : IGameCommand
	{
		public UniqueId TreeId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.BuildingLogic.Collect(TreeId);
		}
	}
}