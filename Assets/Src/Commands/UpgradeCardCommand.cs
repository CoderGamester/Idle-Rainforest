using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct UpgradeCardCommand : IGameCommand
	{
		public GameId Card;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.CardLogic.Upgrade(Card);
		}
	}
}