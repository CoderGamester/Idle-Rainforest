using System;
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
	
	public struct LevelBuildingInfo
	{
		public GameId GameId;
		public LevelBuildingData Data;
		public int NextBracketLevel;
		public int ProductionAmount;
		public float ProductionTime;
		public int UpgradeCost;
		public int AutomateCost;
		public AutomationState AutomationState;

		public DateTime ProductionEndTime => Data.ProductionStartTime.AddSeconds(ProductionTime);
	}
}