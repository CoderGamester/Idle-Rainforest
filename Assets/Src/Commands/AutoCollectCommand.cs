using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct AutoCollectCommand : IGameCommand
	{
		public int Amount;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.CurrencyLogic.AddMainCurrency(Amount);
		}
	}
}