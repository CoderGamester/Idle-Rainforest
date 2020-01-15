using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct IntData
	{
		public GameId GameId;
		public int Int;

		public IntData(GameId gameId, int intValue)
		{
			GameId = gameId;
			Int = intValue;
		}
		
		public override string ToString()
		{
			return $"[{GameId},{Int.ToString()}]";
		}
	}
}