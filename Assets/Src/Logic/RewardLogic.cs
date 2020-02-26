using System;
using Data;
using Events;
using Ids;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IRewardDataProvider
	{
		
	}

	/// <inheritdoc />
	public interface IRewardLogic : IRewardDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void GiveReward(IntData reward);
	}
	
	/// <inheritdoc />
	public class RewardLogic : IRewardLogic
	{
		private readonly IGameInternalLogic _gameLogic;

		private RewardLogic() {}

		public RewardLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}

		/// <inheritdoc />
		public void GiveReward(IntData reward)
		{
			_gameLogic.MessageBrokerService.Publish(new RewardGivingEvent { Reward = reward });
			
			if (reward.Key.IsInGroup(GameIdGroup.Animal))
			{
				_gameLogic.CardLogic.AddCard(reward.Key, reward.Value);
				return;
			}
					
			switch (reward.Key)
			{
				case GameId.MainCurrency:
					_gameLogic.CurrencyLogic.AddMainCurrency(reward.Value);
					break;
				case GameId.SoftCurrency:
					_gameLogic.CurrencyLogic.AddSoftCurrency(reward.Value);
					break;
				case GameId.HardCurrency:
					_gameLogic.CurrencyLogic.AddHardCurrency(reward.Value);
					break;
				default:
					throw new LogicException($"The reward {reward.Key} is not valid");
			}
		}
	}
}