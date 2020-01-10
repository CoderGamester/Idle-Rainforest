using Ids;
using Logic;
using UnityEngine;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CreateBuildingCommand : IGameCommand
	{
		public GameId BuildingId;
		public Vector3 Position;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			var entity = gameLogic.EntityLogic.CreateBuilding(BuildingId, Position);
			gameLogic.GameObjectLogic.LoadGameObject(entity, AddressableId.Prefabs_Building, Position);
		}
	}
}