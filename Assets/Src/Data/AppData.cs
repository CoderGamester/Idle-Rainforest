using System;
using Ids;

namespace Data
{
	/// <summary>
	/// Contains all the data in the scope of the Game's App
	/// </summary>
	[Serializable]
	public class AppData
	{
		public UniqueId UniqueIdCounter = UniqueId.Invalid;
		public DateTime FirstLoginTime = DateTime.UtcNow;
		public DateTime LastLoginTime = DateTime.UtcNow;
		public DateTime LoginTime = DateTime.UtcNow;
	}
}