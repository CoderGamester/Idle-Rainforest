using System;
using Ids;

namespace Data
{
	[Serializable]
	public struct LevelBuildingData
	{
		public UniqueId Id;
		public int Level;
		public DateTime ProductionStartTime;
		public bool IsAutomated;
		public Vector3Serializable Position;
	}
}