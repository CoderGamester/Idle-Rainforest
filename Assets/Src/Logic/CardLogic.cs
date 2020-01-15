using System;
using System.Collections.Generic;
using Configs;
using Data;
using Ids;
using Infos;
using Utils;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface ICardDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		CardInfo GetInfo(GameId card);

		/// <summary>
		/// TODO:
		/// </summary>
		List<CardInfo> GetBuildingCards(GameId building);
	}

	/// <inheritdoc />
	public interface ICardLogic : ICardDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void AddCard(GameId card, int amount);

		/// <summary>
		/// TODO:
		/// </summary>
		void Upgrade(GameId card);
	}
	
	/// <inheritdoc />
	public class CardLogic : ICardLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IIdList<GameId, CardData> _data;
		
		private CardLogic() {}

		public CardLogic(IGameInternalLogic gameLogic, IIdList<GameId, CardData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public CardInfo GetInfo(GameId card)
		{
			var config = _gameLogic.ConfigsProvider.GetConfig<CardConfig>((int) card);
			
			if (!_data.TryGet(card, out CardData data))
			{
				data = new CardData { Id = card, Amount = 0, Level = 0};
			}

			return new CardInfo
			{
				GameId = card,
				Data = data,
				MaxLevel = config.UpgradeCost.Count + 1,
				AmountRequired = config.UpgradeCardsRequired[data.Level - 1],
				UpgradeCost = config.UpgradeCost[data.Level - 1]
			};
		}

		/// <inheritdoc />
		public List<CardInfo> GetBuildingCards(GameId building)
		{
			if (!building.IsInGroup(GameIdGroup.Building))
			{
				throw new ArgumentException($"The id {building} is not part of the {GameIdGroup.Building} group");
			}
			
			var list = new List<CardInfo>();
			var cardConfigs = _gameLogic.ConfigsProvider.GetConfigsList<CardConfig>();

			for (var i = 0; i < cardConfigs.Count; i++)
			{
				if (cardConfigs[i].Building == building)
				{
					list.Add(GetInfo(cardConfigs[i].Id));
				}
			}

			return list;
		}

		/// <inheritdoc />
		public void AddCard(GameId card, int amount)
		{
			if(!_data.TryGet(card, out CardData data))
			{
				data = new CardData { Id = card, Amount = 0, Level = 1 };
				
				_data.Add(data);
			}

			data.Amount += amount;
			
			_data.Set(data);
		}

		/// <inheritdoc />
		public void Upgrade(GameId card)
		{
			var info = GetInfo(card);

			if (info.Data.Level == info.MaxLevel)
			{
				throw new InvalidOperationException($"The card {card} already reached the max level");
			}

			if (info.Data.Amount < info.AmountRequired)
			{
				throw new InvalidOperationException($"The player needs {info.AmountRequired} and only has {info.Data.Amount} to upgrade the card {card}");
			}

			info.Data.Amount -= info.AmountRequired;
			info.Data.Level++;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.UpgradeCost);
			_data.Set(info.Data);
		}
	}
}