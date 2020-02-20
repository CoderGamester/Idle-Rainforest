using System;
using System.Collections.Generic;
using Configs;
using Data;
using Events;
using Ids;
using Infos;
using UnityEngine;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface ILevelTreeDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		IUniqueIdListReader<LevelTreeData> Data { get; }
		
		/// <summary>
		/// TODO:
		/// </summary>
		LevelTreeInfo GetLevelTreeInfo(UniqueId id);
		
		/// <summary>
		/// TODO:
		/// </summary>
		LevelTreeUpgradeInfo GetLevelTreeUpgradeInfo(UniqueId id, uint maxUpgradeAmount);
	}

	/// <inheritdoc />
	public interface ILevelTreeLogic : ILevelTreeDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void Collect(UniqueId id);

		/// <summary>
		/// TODO:
		/// </summary>
		void Upgrade(UniqueId id, uint maxUpgradeAmount);

		/// <summary>
		/// TODO:
		/// </summary>
		void Automate(UniqueId id);
	}
	
	/// <inheritdoc />
	public class LevelTreeLogic : ILevelTreeLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IUniqueIdList<LevelTreeData> _data;

		/// <inheritdoc />
		public IUniqueIdListReader<LevelTreeData> Data => _data;
		
		private LevelTreeLogic() {}

		public LevelTreeLogic(IGameInternalLogic gameLogic, IUniqueIdList<LevelTreeData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public LevelTreeInfo GetLevelTreeInfo(UniqueId id)
		{
			var data = _data.Get(id);
			var gameId = _gameLogic.GameIdLogic.Data.Get(id).GameId;
			var config = _gameLogic.ConfigsProvider.GetConfig<LevelTreeConfig>((int) gameId);

			return new LevelTreeInfo
			{
				GameId = gameId,
				Data = data,
				ProductionAmount = ProductionAmount(data, config),
				ProductionTime = config.ProductionTimeBase,
				AutomateCost = config.AutomationCurrencyRequired,
				AutomateCardRequirement = config.AutomationCardRequired,
				AutomationState = GetBuildingState(data, config),
			};
		}

		/// <inheritdoc />
		public LevelTreeUpgradeInfo GetLevelTreeUpgradeInfo(UniqueId id, uint maxUpgradeAmount)
		{
			var data = _data.Get(id);
			var gameId = _gameLogic.GameIdLogic.Data.Get(id).GameId;
			var config = _gameLogic.ConfigsProvider.GetConfig<LevelTreeConfig>((int) gameId);
			var maxLevel = config.UpgradeBrackets[config.UpgradeBrackets.Count - 1].Key;
			var nextBracket = GetNextLevelBracket(data, config);
			var upgradeCost = config.BuildCost;
			var upgradeAmount = 1;

			if (data.Level > 0)
			{
				var upgradeTo = data.Level + maxUpgradeAmount > nextBracket.Key ? nextBracket.Key - data.Level : (int)maxUpgradeAmount;
				
				upgradeAmount = 0;
				upgradeTo = upgradeTo <= 0 ? (int) maxUpgradeAmount : upgradeTo;
				upgradeCost = FactorialCost(config, data.Level, upgradeTo, ref upgradeAmount);
			}
			
			return new LevelTreeUpgradeInfo
			{
				GameId = gameId,
				Data = data,
				NextBracketLevel = Mathf.Min(nextBracket.Key, maxLevel),
				BracketSize = nextBracket.Value,
				UpgradeCost = upgradeCost,
				UpgradeAmount = upgradeAmount
			};
		}

		/// <inheritdoc />
		public void Collect(UniqueId id)
		{
			var info = GetLevelTreeInfo(id);

			if (info.AutomationState == AutomationState.Automated)
			{
				throw new LogicException($"The building {info.GameId} is already automated and cannot be collected by the player anymore");
			}

			if (_gameLogic.TimeService.DateTimeUtcNow < info.ProductionEndTime)
			{
				throw new LogicException($"The building {info.GameId} is still not ready to collect");
			}
			
			info.Data.ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow;
			
			_gameLogic.MessageBrokerService.Publish(new TreeCollectedEvent { Tree = info.GameId });
			_gameLogic.CurrencyLogic.AddMainCurrency(info.ProductionAmount);
			_data.Set(info.Data);
		}

		/// <inheritdoc />
		public void Upgrade(UniqueId id, uint maxUpgradeAmount)
		{
			var info = GetLevelTreeUpgradeInfo(id, maxUpgradeAmount);

			info.Data.Level += info.UpgradeAmount;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.UpgradeCost);
			_data.Set(info.Data);

			if (info.Data.Level == info.NextBracketLevel)
			{
				var config = _gameLogic.ConfigsProvider.GetConfig<LevelTreeConfig>((int) info.GameId);
			
				for (var i = 0; i < config.UpgradeRewards.Count; i++)
				{
					_gameLogic.RewardLogic.GiveReward(config.UpgradeRewards[i]);
				}
				
				_gameLogic.MessageBrokerService.Publish(new TreeRankedUpEvent { Tree = info.GameId, Level = info.Data.Level});
			}
			
			_gameLogic.MessageBrokerService.Publish(new LevelTreeUpgradedEvent { Tree = info.GameId, NewLevel = info.Data.Level});
		}

		/// <inheritdoc />
		public void Automate(UniqueId id)
		{
			var info = GetLevelTreeInfo(id);

			if (info.AutomationState != AutomationState.ReadyToAutomate)
			{
				throw new LogicException($"The building {info.GameId} is still not ready to be automated");
			}

			info.Data.IsAutomated = true;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.AutomateCost);
			_data.Set(info.Data);
			
			_gameLogic.MessageBrokerService.Publish(new TreeAutomatedEvent { Tree = info.GameId });
		}

		private AutomationState GetBuildingState(LevelTreeData data, LevelTreeConfig config)
		{
			var state = AutomationState.ReadyToAutomate;
			
			if (data.IsAutomated)
			{
				state = AutomationState.Automated;
			}
			else if(_gameLogic.CurrencyLogic.MainCurrencyAmount < config.AutomationCurrencyRequired)
			{
				state = AutomationState.MissingRequirements;
			}
			else
			{
				var cardInfo = _gameLogic.CardLogic.GetInfo(config.Tree);

				state = cardInfo.Data.Level < config.AutomationCardRequired.Value
					? AutomationState.MissingRequirements
					: AutomationState.ReadyToAutomate;
			}

			return state;
		}

		private IntPairData GetNextLevelBracket(LevelTreeData data, LevelTreeConfig config)
		{
			var nextBracket = new IntPairData(int.MaxValue, 0);

			for (var i = 0; i < config.UpgradeBrackets.Count; i++)
			{
				var bracket = config.UpgradeBrackets[i];
				
				if (data.Level < bracket.Key && bracket.Key < nextBracket.Key)
				{
					nextBracket.Key = (Mathf.FloorToInt((float) data.Level / bracket.Value) + 1) * bracket.Value;
					nextBracket.Value = bracket.Value;
				}
			}

			return nextBracket;
		}

		private int ProductionAmount(LevelTreeData data, LevelTreeConfig config)
		{
			var amount = config.ProductionAmountBase + config.ProductionAmountIncrease * data.Level;
			var animalCards = _gameLogic.CardLogic.GetAnimalCards(config.Tree);
			var treeCard = _gameLogic.CardLogic.GetInfo(config.Tree);
			var totalAmount = amount + amount * treeCard.ProductionBonus;

			foreach (var card in animalCards)
			{
				totalAmount += amount * card.ProductionBonus;
			}

			return totalAmount;
		}

		private int FactorialCost(LevelTreeConfig config, int level, int amount, ref int upgradeAmount)
		{
			var cost = 0;
			var totalCost = config.UpgradeCostBase + config.UpgradeCostIncrease * level;
			
			while (amount > 0 && totalCost + cost <= _gameLogic.CurrencyDataProvider.MainCurrencyAmount)
			{
				upgradeAmount++;
				level++;
				amount--;
				totalCost += cost;
				cost = config.UpgradeCostBase + config.UpgradeCostIncrease * level;
			}
			
			return totalCost;
		}
	}
}