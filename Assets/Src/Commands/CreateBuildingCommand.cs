using Ids;
using Logic;
using UnityEngine;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CreateBuildingCommand : IGameCommand
	{
		public GameId BuildingType;
		public Vector3 Position;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.EntityLogic.CreateBuilding(BuildingType, Position);
		}
	}
}