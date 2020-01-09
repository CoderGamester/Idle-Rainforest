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
		public int MainCurrency;
		public int SoftCurrency;
		public int HardCurrency;
		
		public List<ResourceData> Resources = new List<ResourceData>();
		public List<BuildingData> Buildings = new List<BuildingData>();
	}
}