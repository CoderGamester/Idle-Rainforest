using System;
using System.Collections.Generic;
using Data;
using Ids;

namespace Infos
{
	public enum AutomationState
	{
		MissingRequirements,
		Ready,
		Automated
	}
	
	public struct BuildingInfo
	{
		public GameId GameId;
		public BuildingData Data;
		public int NextBracketLevel;
		public int MaxLevel;
		public int ProductionAmount;
		public float ProductionTime;
		public int UpgradeCost;
		public int AutomateCost;
		public AutomationState AutomationState;
		public List<GameId> CardList;

		public DateTime ProductionEndTime => Data.ProductionStartTime.AddSeconds(ProductionTime);
	}
}