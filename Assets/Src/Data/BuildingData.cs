using System;
using Ids;
using UnityEngine;

namespace Data
{
	[Serializable]
	public struct BuildingData : ISerializationCallbackReceiver
	{
		public GameId GameId;
		public int Level;
		public DateTime ProductionStartTime;
		
		[SerializeField] private Vector3Serializable _position;

		public Vector3 Position { get; set; }
		
		public void OnBeforeSerialize()
		{
			Position = _position;
		}

		public void OnAfterDeserialize()
		{
			_position = Position;
		}
	}
}