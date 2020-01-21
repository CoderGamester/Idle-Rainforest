using System.Collections.Generic;
using Commands;
using Data;
using GameLovers.Services;
using Infos;
using Logic;
using Services;
using UnityEngine;

namespace Systems
{
	/// <summary>
	/// TODO:
	/// </summary>
	public class AutoCollectSystem : ITickSystem
	{
		private IGameServices _services;
		private IGameDataProvider _dataProvider;
		private IList<LevelBuildingData> _data;
		
		private AutoCollectSystem() {}
		
		public AutoCollectSystem(IList<LevelBuildingData> data)
		{
			_services = MainInstaller.Resolve<IGameServices>();
			_dataProvider = MainInstaller.Resolve<IGameDataProvider>();
			_data = data;
		}
		
		/// <inheritdoc />
		public void Tick()
		{
			for (var i = 0; i < _data.Count; i++)
			{
				var info = _dataProvider.BuildingDataProvider.GetLevelBuildingInfo(_data[i].Id);
				
				if (info.AutomationState == AutomationState.Automated && _services.TimeService.DateTimeUtcNow > info.ProductionEndTime)
				{
					var times = Mathf.FloorToInt((float) (info.ProductionEndTime - info.Data.ProductionStartTime).TotalSeconds / info.ProductionTime);
					
					info.Data.ProductionStartTime = info.Data.ProductionStartTime.AddSeconds(info.ProductionTime * times);
					
					_services.CommandService.ExecuteCommand(new AutoCollectCommand { Amount = times * info.ProductionAmount });
				}

				_data[i] = info.Data;
			}
		}
	}
}