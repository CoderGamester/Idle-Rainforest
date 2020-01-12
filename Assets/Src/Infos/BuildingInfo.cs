using System;
using Data;
using Ids;

namespace Infos
{
	public struct BuildingInfo
	{
		public UniqueId Unique;
		public GameId GameId;
		public BuildingData Data;
		public float ProductionAmount;
		public float ProductionTime;
		public int UpgradeCost;

		public DateTime ProductionStartTime => Data.ProductionStartTime;
		public DateTime ProductionEndTime => ProductionStartTime.AddSeconds(ProductionTime);
	}
}