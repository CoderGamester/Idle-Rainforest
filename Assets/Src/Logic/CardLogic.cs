using System;
using System.Collections.Generic;
using Configs;
using Data;
using Events;
using GameLovers;
using GameLovers.ConfigsContainer;
using Ids;
using Infos;

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
		IIdListReader<GameId, CardData> Data { get; }
		
		/// <summary>
		/// TODO:
		/// </summary>
		CardInfo GetInfo(GameId card);

		/// <summary>
		/// TODO:
		/// </summary>
		List<CardInfo> GetAnimalCards();

		/// <summary>
		/// TODO:
		/// </summary>
		List<CardInfo> GetTreeCards();

		/// <summary>
		/// TODO:
		/// </summary>
		List<CardInfo> GetAnimalCards(GameId tree);

		/// <summary>
		/// TODO:
		/// </summary>
		List<CardInfo> GetAllCards();
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

		/// <inheritdoc />
		public IIdListReader<GameId, CardData> Data => _data;
		
		private CardLogic() {}

		public CardLogic(IGameInternalLogic gameLogic, IdList<GameId, CardData> data)
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

			var index = data.Level == 0 ? 0 : data.Level - 1;

			return new CardInfo
			{
				GameId = card,
				Data = data,
				MaxLevel = config.UpgradeCost.Count + 1,
				AmountRequired = config.UpgradeCardsRequired[index],
				UpgradeCost = config.UpgradeCost[index],
				ProductionBonus = data.Level == 0 ? 1 : config.LevelBonus[index]
			};
		}

		/// <inheritdoc />
		public List<CardInfo> GetTreeCards()
		{
			var list = new List<CardInfo>();
			var cards = _gameLogic.ConfigsProvider.GetConfigsList<CardConfig>();

			for (var i = 0; i < cards.Count; i++)
			{
				if (cards[i].Id.IsInGroup(GameIdGroup.Tree))
				{
					list.Add(GetInfo(cards[i].Id));
				}
			}
			
			list.Sort((elem1, elem2) => ((int)elem1.GameId).CompareTo((int)elem2.GameId));

			return list;
		}

		/// <inheritdoc />
		public List<CardInfo> GetAnimalCards()
		{
			var list = new List<CardInfo>();
			var cards = _gameLogic.ConfigsProvider.GetConfigsList<CardConfig>();

			for (var i = 0; i < cards.Count; i++)
			{
				if (cards[i].Id.IsInGroup(GameIdGroup.Animal))
				{
					list.Add(GetInfo(cards[i].Id));
				}
			}
			
			list.Sort((elem1, elem2) => ((int)elem1.GameId).CompareTo((int)elem2.GameId));

			return list;
		}

		/// <inheritdoc />
		public List<CardInfo> GetAnimalCards(GameId tree)
		{
			if (!tree.IsInGroup(GameIdGroup.Tree))
			{
				throw new LogicException($"The id {tree} is not part of the {GameIdGroup.Tree} group");
			}
			
			var list = new List<CardInfo>();
			var animals = _gameLogic.ConfigsProvider.GetConfigsList<AnimalConfig>();

			for (var i = 0; i < animals.Count; i++)
			{
				if (animals[i].Tree == tree)
				{
					list.Add(GetInfo(animals[i].Id));
				}
			}
			
			list.Sort((elem1, elem2) => ((int)elem1.GameId).CompareTo((int)elem2.GameId));

			return list;
		}

		/// <inheritdoc />
		public List<CardInfo> GetAllCards()
		{
			var list = new List<CardInfo>();
			var cardConfigs = _gameLogic.ConfigsProvider.GetConfigsList<CardConfig>();

			for (var i = 0; i < cardConfigs.Count; i++)
			{
				list.Add(GetInfo(cardConfigs[i].Id));
			}
			
			list.Sort((elem1, elem2) => ((int)elem1.GameId).CompareTo((int)elem2.GameId));
			
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
			_gameLogic.MessageBrokerService.Publish(new CardCollectedEvent { Card = card, Amount = amount});
		}

		/// <inheritdoc />
		public void Upgrade(GameId card)
		{
			var info = GetInfo(card);

			if (info.Data.Level == info.MaxLevel)
			{
				throw new LogicException($"The card {card} already reached the max level");
			}

			if (info.Data.Amount < info.AmountRequired)
			{
				throw new LogicException($"The player needs {info.AmountRequired} and only has {info.Data.Amount} to upgrade the card {card}");
			}

			info.Data.Amount -= info.AmountRequired;
			info.Data.Level++;
			
			_gameLogic.CurrencyLogic.DeductHardCurrency(info.UpgradeCost);
			_data.Set(info.Data);
			_gameLogic.MessageBrokerService.Publish(new CardUpgradedEvent { Card = card, NewLevel = info.Data.Level });
		}
	}
}