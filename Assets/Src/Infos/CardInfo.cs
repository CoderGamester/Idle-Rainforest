using Data;
using Ids;

namespace Infos
{
	public struct CardInfo
	{
		public GameId GameId;
		public CardData Data;
		public GameId Tree;
		public int AmountRequired;
		public int MaxLevel;
		public int UpgradeCost;
		public int ProductionBonus;
	}
}