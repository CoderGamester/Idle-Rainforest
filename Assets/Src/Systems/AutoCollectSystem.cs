using System;
using Commands;
using Data;
using Data.ComponentData;
using GameLovers.Services;
using Ids;
using Services;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class AutoCollectSystem : SystemBase
	{
		private IGameServices _services;

		protected override void OnStartRunning()
		{
			_services = MainInstaller.Resolve<IGameServices>();
		}

		protected override void OnUpdate()
		{
			var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			var entities = entityManager.GetAllEntities();
			var timeNow = _services.TimeService.DateTimeUtcNow;

			foreach (var entity in entities)
			{
				if (!entityManager.HasComponent<AutoLevelTreeData>(entity))
				{
					continue;
				}

				var data = entityManager.GetComponentData<AutoLevelTreeData>(entity);
				
				if (timeNow < data.ProductionEndTime)
				{
					continue;
				}

				var timeSpan = timeNow - data.ProductionStartTime;
				var count = (int) math.floor(timeSpan.TotalSeconds / data.ProductionTime);

				data.ProductionStartTime = data.ProductionStartTime.AddSeconds(data.ProductionTime * count);
				
				entityManager.SetComponentData(entity, data);
				_services.CommandService.ExecuteCommand(new AutoCollectCommand { Id = data.Id, CollectCount = count });
			}
		}
	}
}