using System;
using Ids;
using UnityEngine;

namespace Data
{
	[Serializable]
	public struct BuildingData : ISerializationCallbackReceiver
	{
		[SerializeField]
		private Vector3Serializable _position;
		
		public GameId Id;

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