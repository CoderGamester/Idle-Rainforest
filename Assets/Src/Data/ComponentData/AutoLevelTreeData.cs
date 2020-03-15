using System;
using Ids;
using Unity.Entities;

namespace Data.ComponentData
{
	public struct AutoLevelTreeData : IComponentData
	{
		public UniqueId Id;
		public DateTime ProductionStartTime;
		public float ProductionTime;

		public DateTime ProductionEndTime => ProductionStartTime.AddSeconds(ProductionTime);
	}
}