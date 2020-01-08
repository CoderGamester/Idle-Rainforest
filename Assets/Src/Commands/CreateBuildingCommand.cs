using GameLovers.AddressableIdsScriptGenerator;
using GameLovers.LoaderExtension;
using Ids;
using Logic;
using MonoComponent;
using UnityEngine;

namespace Commands
{
	public struct CreateBuildingCommand : IGameCommand
	{
		public GameId BuildingId;
		public Vector3 Position;
		
		/// <inheritdoc />
		public async void Execute(IGameLogic gameLogic)
		{
			var entity = gameLogic.EntityLogic.CreateBuilding(BuildingId, Position);
			var address = gameLogic.Configs.GetConfig<AddressableConfig>((int) BuildingId).Address;
			var task = LoaderUtil.LoadAssetAsync<GameObject>(address, false);

			await task;

			var gameObject = Object.Instantiate(task.Result, Position, Quaternion.identity);
			
			gameObject.GetComponent<EntityMonoComponent>().Entity = entity;
		}
	}
}