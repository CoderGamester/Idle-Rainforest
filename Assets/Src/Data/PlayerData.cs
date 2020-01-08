using System;
using System.Collections.Generic;

namespace Data
{
	/// <summary>
	/// Contains all the data in the scope of the Player 
	/// </summary>
	[Serializable]
	public class PlayerData
	{
		public List<BuildingData> Buildings = new List<BuildingData>();
	}
}