using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct IntData
	{
		public GameId GameId;
		public int IntValue;

		public IntData(GameId gameId, int intValue)
		{
			GameId = gameId;
			IntValue = intValue;
		}
		
		public override string ToString()
		{
			return $"[{GameId},{IntValue.ToString()}]";
		}
	}
	
	[Serializable]
	public struct IntPairData
	{
		public int IntKey;
		public int IntValue;

		public IntPairData(int intKey, int intValue)
		{
			IntKey = intKey;
			IntValue = intValue;
		}
		
		public override string ToString()
		{
			return $"[{IntKey.ToString()},{IntValue.ToString()}]";
		}
	}
}