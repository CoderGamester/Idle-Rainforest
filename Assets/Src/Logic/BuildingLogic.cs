using System;
using System.Threading;
using Configs;
using Data;
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
		IUniqueIdListReader<BuildingData> Data { get; }
		
		/// <summary>
		/// TODO:
		/// </summary>
		BuildingInfo GetInfo(UniqueId id);
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
		private readonly IUniqueIdList<BuildingData> _data;
		
		private BuildingLogic() {}

		public BuildingLogic(IGameInternalLogic gameLogic, IUniqueIdList<BuildingData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public IUniqueIdListReader<BuildingData> Data => _data;

		/// <inheritdoc />
		public BuildingInfo GetInfo(UniqueId id)
		{
			var data = _data.Get(id);
			var gameId = _gameLogic.GameIdLogic.Data.Get(id).GameId;
			var config = _gameLogic.ConfigsProvider.GetConfig<BuildingConfig>((int) gameId);
			var buildingCards = _gameLogic.CardDataProvider.GetBuildingCards(gameId);
			var maxLevel = config.UpgradeBrackets[config.UpgradeBrackets.Count - 1].IntValue;
			var nextBracket = maxLevel;
			var state = AutomationState.Ready;

			for (var i = 0; i < config.UpgradeBrackets.Count; i++)
			{
				var bracket = config.UpgradeBrackets[i];
				if (data.Level < bracket.IntValue)
				{
					nextBracket = bracket.IntValue - Mathf.FloorToInt(bracket.IntValue - data.Level / (float) bracket.IntKey);
				}
			}

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
				foreach (var card in buildingCards)
				{
					if (card.Data.Level < config.AutomationCardLevelRequired)
					{
						state = AutomationState.MissingRequirements;
						break;
					}
				}
			}
			
			return new BuildingInfo
			{
				GameId = gameId,
				Data = data,
				NextBracketLevel = nextBracket,
				MaxLevel = maxLevel,
				ProductionAmount = config.ProductionAmountBase + config.ProductionAmountIncrease * data.Level,
				ProductionTime = config.ProductionTimeBase,
				UpgradeCost = config.UpgradeCostBase + config.UpgradeCostIncrease * data.Level,
				AutomateCost = config.AutomationCurrencyRequired,
				AutomationState = state
			};
		}

		/// <inheritdoc />
		public void Collect(UniqueId id)
		{
			var info = GetInfo(id);

			if (_gameLogic.TimeService.DateTimeUtcNow < info.ProductionEndTime)
			{
				throw new InvalidOperationException($"The building {info.GameId} is still not ready to collect");
			}

			if (info.AutomationState == AutomationState.Automated)
			{
				throw new InvalidOperationException($"The building {info.GameId} is already automated and cannot be collected by the player anymore");
			}
			
			info.Data.ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow;
			
			_gameLogic.CurrencyLogic.AddMainCurrency(info.ProductionAmount);
			_data.Set(info.Data);
		}

		/// <inheritdoc />
		public void Upgrade(UniqueId id)
		{
			var info = GetInfo(id);
			var config = _gameLogic.ConfigsProvider.GetConfig<BuildingConfig>((int) info.GameId);

			info.Data.Level++;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.UpgradeCost);
			_data.Set(info.Data);

			if (info.Data.Level != info.NextBracketLevel)
			{
				return;
			}
			
			for (var i = 0; i < config.UpgradeRewards.Count; i++)
			{
				if (config.UpgradeRewards[i].GameId.IsInGroup(GameIdGroup.Card))
				{
					_gameLogic.CardLogic.AddCard(config.UpgradeRewards[i].GameId, config.UpgradeRewards[i].IntValue);
					continue;
				}
					
				switch (config.UpgradeRewards[i].GameId)
				{
					case GameId.MainCurrency:
						_gameLogic.CurrencyLogic.AddMainCurrency(config.UpgradeRewards[i].IntValue);
						break;
					case GameId.SoftCurrency:
						_gameLogic.CurrencyLogic.AddSoftCurrency(config.UpgradeRewards[i].IntValue);
						break;
					case GameId.HardCurrency:
						_gameLogic.CurrencyLogic.AddHardCurrency(config.UpgradeRewards[i].IntValue);
						break;
					default:
						throw new ArgumentOutOfRangeException($"Wrong reward {config.UpgradeRewards[i].GameId} for the upgrade {info.GameId}");
				}
			}
		}

		/// <inheritdoc />
		public void Automate(UniqueId id)
		{
			var info = GetInfo(id);

			if (info.AutomationState != AutomationState.Ready)
			{
				throw new InvalidOperationException($"The building {info.GameId} is still not ready to be automated");
			}

			info.Data.IsAutomated = true;
			
			_gameLogic.CurrencyLogic.DeductMainCurrency(info.AutomateCost);
			_data.Set(info.Data);
		}
	}
}