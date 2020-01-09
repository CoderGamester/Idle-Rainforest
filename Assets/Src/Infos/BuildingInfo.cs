using System;
using Ids;
using UnityEngine;

namespace Infos
{
	public struct BuildingInfo
	{
		public EntityId Entity;
		public GameId GameId;
		public int Level;
		public Vector3 Position;
		public float ProductionAmount;
		public float ProductionTime;
		public DateTime ProductionStartTime;

		public DateTime ProductionEndTime => ProductionStartTime.AddSeconds(ProductionTime);
	}
}