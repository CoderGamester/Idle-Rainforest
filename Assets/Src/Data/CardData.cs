using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct CardData
	{
		public GameId Id;
		public int Level;
		public int Amount;
	}
}