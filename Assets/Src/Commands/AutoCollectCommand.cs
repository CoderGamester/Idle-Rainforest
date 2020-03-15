using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct AutoCollectCommand : IGameCommand
	{
		public UniqueId Id;
		public int CollectCount;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.LevelTreeLogic.AutoCollect(Id, CollectCount);
		}
	}
}