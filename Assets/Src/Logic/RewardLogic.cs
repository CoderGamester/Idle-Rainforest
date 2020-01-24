using System;
using Data;
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
			if (reward.GameId.IsInGroup(GameIdGroup.Card))
			{
				_gameLogic.CardLogic.AddCard(reward);
				return;
			}
					
			switch (reward.GameId)
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
					throw new ArgumentOutOfRangeException($"The reward {reward.GameId} is not valid");
			}
		}
	}
}