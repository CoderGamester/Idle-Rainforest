using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct IntData
	{
		public GameId GameId;
		public int Value;

		public IntData(GameId gameId, int value)
		{
			GameId = gameId;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{GameId},{Value.ToString()}]";
		}
	}
	
	[Serializable]
	public struct IntPairData
	{
		public int Key;
		public int Value;

		public IntPairData(int key, int value)
		{
			Key = key;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{Key.ToString()},{Value.ToString()}]";
		}
	}
	
	[Serializable]
	public struct FloatData
	{
		public GameId GameId;
		public float Value;

		public FloatData(GameId gameId, float value)
		{
			GameId = gameId;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{GameId},{Value.ToString()}]";
		}
	}
	
	[Serializable]
	public struct FloatPair
	{
		public float Key;
		public float Value;

		public FloatPair(float key, float value)
		{
			Key = key;
			Value = value;
		}
		
		public override string ToString()
		{
			return $"[{Key.ToString()},{Value.ToString()}]";
		}
	}
}