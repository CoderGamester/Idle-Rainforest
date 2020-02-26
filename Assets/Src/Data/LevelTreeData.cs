using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct LevelTreeData
	{
		public UniqueId Id;
		public int Level;
		public DateTime ProductionStartTime;
		public bool IsAutomated;
		public int Position;
	}
}