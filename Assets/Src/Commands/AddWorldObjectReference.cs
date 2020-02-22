using Ids;
using Logic;
using UnityEngine;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct AddWorldObjectReference : IGameCommand
	{
		public GameObject WorldObjectReference;
		public GameId GameId;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			if (!gameLogic.GameIdLogic.TryGetData(GameId, out var data))
			{
				data.Id = gameLogic.EntityLogic.CreateWorldReference(GameId);
				data.GameId = GameId;
			}

			gameLogic.WorldObjectLogic.AddWorldObject(data.Id, WorldObjectReference);
		}
	}
}