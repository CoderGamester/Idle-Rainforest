using System;
using System.Collections.Generic;
using Data;
using Ids;

namespace Infos
{
	public enum AutomationState
	{
		MissingRequirements,
		ReadyToAutomate,
		Automated
	}
	
	public struct LevelTreeInfo
	{
		public GameId GameId;
		public LevelTreeData Data;
		public int ProductionAmount;
		public float ProductionTime;
		public int AutomateCost;
		public IntData AutomateCardRequirement;
		public AutomationState AutomationState;
		public List<CardInfo> Cards;

		public DateTime ProductionEndTime => Data.ProductionStartTime.AddSeconds(ProductionTime);
	}

	public struct LevelTreeUpgradeInfo
	{
		public GameId GameId;
		public LevelTreeData Data;
		public int BracketSize;
		public int NextBracketLevel;
		public int UpgradeCost;
		public int UpgradeAmount;

		public int UpgradeLevel => Data.Level + UpgradeAmount;
	}
}