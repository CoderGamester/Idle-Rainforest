using System;
using System.Collections.Generic;
using Configs;
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
		BuildingInfo GetInfo(EntityId entity);
	}

	/// <inheritdoc />
	public interface IBuildingLogic : IBuildingDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void Collect(EntityId entity);

		/// <summary>
		/// TODO:
		/// </summary>
		void Upgrade(EntityId entity);
	}
	
	/// <inheritdoc />
	public class BuildingLogic : IBuildingLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<EntityId, int> _data;
		
		private BuildingLogic() {}

		public BuildingLogic(IGameInternalLogic gameLogic, IDictionary<EntityId, int> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public BuildingInfo GetInfo(EntityId entity)
		{
			var data = _gameLogic.DataProviderLogic.PlayerData.Buildings[_data[entity]];
			var config = _gameLogic.ConfigsProvider.GetConfig<BuildingConfig>((int) data.GameId);
			
			return new BuildingInfo
			{
				Entity = entity,
				GameId = data.GameId,
				Level = data.Level,
				Position = data.Position,
				ProductionAmount = config.ProductionAmountBase + config.ProductionAmountIncrease * data.Level,
				ProductionTime = config.ProductionTimeBase,
				ProductionStartTime = data.ProductionStartTime,
				UpgradeCost = config.UpgradeCostBase + config.UpgradeCostIncrease * data.Level,
			};
		}

		/// <inheritdoc />
		public void Collect(EntityId entity)
		{
			if (!_data.TryGetValue(entity, out int index))
			{
				throw new ArgumentException($"There is no building with the entity {entity.ToString()}");
			}
			
			var data = _gameLogic.DataProviderLogic.PlayerData.Buildings[index];
			var info = GetInfo(entity);

			if (_gameLogic.TimeService.DateTimeUtcNow < info.ProductionEndTime)
			{
				throw new InvalidOperationException($"The building {data.GameId} is still not ready to collect");
			}
			
			data.ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow;

			_gameLogic.CurrencyLogic.AddMainCurrency(Mathf.RoundToInt(info.ProductionAmount));
			_gameLogic.DataProviderLogic.PlayerData.Buildings[_data[entity]] = data;
		}

		/// <inheritdoc />
		public void Upgrade(EntityId entity)
		{
			if (!_data.TryGetValue(entity, out int index))
			{
				throw new ArgumentException($"There is no building with the entity {entity.ToString()}");
			}
			
			var data = _gameLogic.DataProviderLogic.PlayerData.Buildings[index];
			var info = GetInfo(entity);

			_gameLogic.CurrencyLogic.DeductMainCurrency(info.UpgradeCost);

			data.Level++;

			_gameLogic.DataProviderLogic.PlayerData.Buildings[index] = data;
			
			_gameLogic.MessageBrokerService.Publish(new BuildingUpgradedEvent { Entity = entity });
		}
	}
}