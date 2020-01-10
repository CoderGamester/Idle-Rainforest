using System;

namespace Data
{
	/// <summary>
	/// Contains all the data in the scope of the Game's App
	/// </summary>
	[Serializable]
	public class AppData
	{
		public DateTime LastLoginTime = DateTime.Now;
		public DateTime FirstLoginTime = DateTime.Now;
	}
}