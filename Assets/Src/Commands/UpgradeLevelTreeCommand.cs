using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct UpgradeLevelTreeCommand : IGameCommand
	{
		public UniqueId TreeId;
		public uint UpgradeSize;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.LevelTreeLogic.Upgrade(TreeId, UpgradeSize);
		}
	}
}