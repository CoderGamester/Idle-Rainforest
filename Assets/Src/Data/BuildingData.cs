using System;
using Ids;
using UnityEngine;

namespace Data
{
	[Serializable]
	public struct BuildingData
	{
		public UniqueId Id;
		public int Level;
		public DateTime ProductionStartTime;
		public bool IsAutomated;
		
		[SerializeField] private Vector3Serializable _position;

		public Vector3 Position
		{
			get => _position;
			set => _position = value;
		}
	}
}