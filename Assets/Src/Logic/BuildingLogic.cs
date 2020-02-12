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
	public interface IBuildingDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		IUniqueIdListReader<LevelBuildingData> Data { get; }
		
		/// <summary>
		/// TODO:
		/// </summary>
		LevelTreeInfo GetLevelBuildingInfo(UniqueId id);
	}

	/// <inheritdoc />
	public interface IBuildingLogic : IBuildingDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void Collect(UniqueId id);

		/// <summary>
		/// TODO:
		/// </summary>
		void Upgrade(UniqueId id);

		/// <summary>
		/// TODO:
		/// </summary>
		void Automate(UniqueId id);
	}
	
	/// <inheritdoc />
	public class BuildingLogic : IBuildingLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IUniqueIdList<LevelBuildingData> _data;

		/// <inheritdoc />
		public IUniqueIdListReader<LevelBuildingData> Data => _data;
		
		private BuildingLogic() {}

		public BuildingLogic(IGameInternalLogic gameLogic, IUniqueIdList<LevelBuildingData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public LevelTreeInfo GetLevelBuildingInfo(UniqueId id)
		{
			var data = _data.Get(id);
			var gameId = _gameLogic.GameIdLogic.Data.Get(id).GameId;
			var config = _gameLogic.ConfigsProvider.GetConfig<LevelTreeConfig>((int) gameId);
			var maxLevel = config.UpgradeBrackets[config.UpgradeBrackets.Count - 1].Key;
			var buildingCards = _gameLogic.CardDataProvider.GetTreeCards(config.Tree);
			var nextBracket = GetNextLevelBracket(data, config);
			var upgradeCost = data.Level == 0 ? config.BuildCost : config.UpgradeCostBase + config.UpgradeCostIncrease * data.Level;

			return new LevelTreeInfo
			{
				GameId = gameId,
				Data = data,
				NextBracketLevel = Mathf.Min(nextBracket.Key, maxLevel),
				BracketSize = nextBracket.Value,
				ProductionAmount = ProductionAmount(data, config),
				ProductionTime = config.ProductionTimeBase,
				UpgradeCost = upgradeCost,
				AutomateCost = config.AutomationCurrencyRequired,
				AutomateCardRequirement = config.AutomationCardRequired,
				AutomationState = GetBuildingState(data, config, buildingCards),
				Cards = buildingCards
			};
		}

		/// <inheritdoc />
		public void Collect(UniqueId id)
		{
			var info = GetLevelBuildingInfo(id);

			if (info.AutomationState == AutomationState.Automated)
			{
				throw new LogicException($"The building {info.GameId} is already automated and cannot be collected by the player anymore");
			}

			if (_gameLogic.TimeService.DateTimeUtcNow < info.ProductionEndTime)
			{
				throw new LogicException($"The building {info.GameId} is still not ready to collect");
			}
			
			info.Data.ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow;
			
			_gameLogic.MessageBrokerService.Publish(new BuildingCollectedEvent { Building = info.GameId });
			_gameLogic.CurrencyLogic.AddMainCurrency(info.ProductionAmount);
			_data.Set(info.Data);
		}

		/// <inheritdoc />
		public void Upgrade(UniqueId id)
		{
			var info = GetLevelBuildingInfo(id);

			info.Data.Level++;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.UpgradeCost);
			_data.Set(info.Data);

			if (info.Data.Level == info.NextBracketLevel)
			{
				var config = _gameLogic.ConfigsProvider.GetConfig<LevelTreeConfig>((int) info.GameId);
			
				for (var i = 0; i < config.UpgradeRewards.Count; i++)
				{
					_gameLogic.RewardLogic.GiveReward(config.UpgradeRewards[i]);
				}
			}
			
			_gameLogic.MessageBrokerService.Publish(new LevelBuildingUpgradedEvent { Building = info.GameId, NewLevel = info.Data.Level});
		}

		/// <inheritdoc />
		public void Automate(UniqueId id)
		{
			var info = GetLevelBuildingInfo(id);

			if (info.AutomationState != AutomationState.ReadyToAutomate)
			{
				throw new LogicException($"The building {info.GameId} is still not ready to be automated");
			}

			info.Data.IsAutomated = true;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.AutomateCost);
			_data.Set(info.Data);
			
			_gameLogic.MessageBrokerService.Publish(new BuildingAutomatedEvent { Building = info.GameId });
		}

		private AutomationState GetBuildingState(LevelBuildingData data, LevelTreeConfig config, List<CardInfo> buildingCards)
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
				var cardInfo = buildingCards.Find(card => card.GameId == config.AutomationCardRequired.GameId);

				state = cardInfo.Data.Level < config.AutomationCardRequired.Value
					? AutomationState.MissingRequirements
					: AutomationState.ReadyToAutomate;
			}

			return state;
		}

		private IntPairData GetNextLevelBracket(LevelBuildingData data, LevelTreeConfig config)
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

		private int ProductionAmount(LevelBuildingData data, LevelTreeConfig config)
		{
			var amount = config.ProductionAmountBase + config.ProductionAmountIncrease * data.Level;
			var cards = _gameLogic.CardLogic.GetTreeCards(config.Tree);
			var totalAmount = amount;

			foreach (var card in cards)
			{
				totalAmount += amount * card.ProductionBonus;
			}

			return totalAmount;
		}
	}
}