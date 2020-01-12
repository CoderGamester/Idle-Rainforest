using System;
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
			
			return new BuildingInfo
			{
				Unique = id,
				GameId = gameId,
				Data = data,
				ProductionAmount = config.ProductionAmountBase + config.ProductionAmountIncrease * data.Level,
				ProductionTime = config.ProductionTimeBase,
				UpgradeCost = config.UpgradeCostBase + config.UpgradeCostIncrease * data.Level
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
			
			info.Data.ProductionStartTime = _gameLogic.TimeService.DateTimeUtcNow;
			
			_gameLogic.CurrencyLogic.AddMainCurrency(Mathf.RoundToInt(info.ProductionAmount));
			_data.Set(info.Data);
		}

		/// <inheritdoc />
		public void Upgrade(UniqueId id)
		{
			var info = GetInfo(id);

			_gameLogic.CurrencyLogic.DeductMainCurrency(info.UpgradeCost);

			info.Data.Level++;
			
			_data.Set(info.Data);
		}
	}
}