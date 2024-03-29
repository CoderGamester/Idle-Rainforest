using System;
using System.Collections.Generic;
using Ids;
using UnityEngine;

namespace Data
{
	/// <summary>
	/// Contains all the data in the scope of the Player 
	/// </summary>
	[Serializable]
	public class PlayerData
	{
		public List<GameIdData> GameIds = new List<GameIdData>();
		public List<CardData> Cards = new List<CardData>();
	}
}