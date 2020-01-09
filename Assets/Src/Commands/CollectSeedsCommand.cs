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
			var seeds = gameLogic.CurrencyLogic.MainCurrencyAmount;
			
			gameLogic.BuildingLogic.Collect(Entity);
			gameLogic.MessageBrokerService.Publish(new SeedsCollectedEvent
			{
				NewValue = gameLogic.CurrencyLogic.MainCurrencyAmount,
				OldValue = seeds
			});
		}
	}
}