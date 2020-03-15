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
			var timeNow = _services.TimeService.DateTimeUtcNow;
			var queue = new NativeQueue<AutoCollectCommand>();

			Entities.ForEach((ref AutoLevelTreeData data) =>
				{
					if (timeNow < data.ProductionEndTime)
					{
						return;
					}

					var timeSpan = timeNow - data.ProductionStartTime;
					var count = (int) math.floor(timeSpan.TotalSeconds / data.ProductionTime);

					data.ProductionStartTime = data.ProductionStartTime.AddSeconds(data.ProductionTime * count);

					queue.Enqueue(new AutoCollectCommand { Id = data.Id, CollectCount = count });
				})
				.ScheduleParallel();

			while (queue.Count > 0)
			{
				_services.CommandService.ExecuteCommand(queue.Dequeue());
			}
		}
	}
}