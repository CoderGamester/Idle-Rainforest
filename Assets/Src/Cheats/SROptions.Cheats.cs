using System;
using System.ComponentModel;
using GameLovers.Services;
using Services;
using UnityEngine;

namespace Cheats
{
	public partial class SROptions
	{
		[Category("Data")]
		public void ResetAllData()
		{
			PlayerPrefs.DeleteAll();
		}
		
		[Category("Time")]
		public void Add1Day()
		{
			var timeManipulator = MainInstaller.Resolve<IGameServices>().TimeService as ITimeManipulator;
			var timeNow = timeManipulator.DateTimeUtcNow;
			
			timeManipulator.AddTime((float) (timeNow.AddDays(1) - timeNow).TotalSeconds);
		}
		
		[Category("Time")]
		public void Add1Hour()
		{
			var timeManipulator = MainInstaller.Resolve<IGameServices>().TimeService as ITimeManipulator;
			var timeNow = timeManipulator.DateTimeUtcNow;
			
			timeManipulator.AddTime((float) (timeNow.AddHours(1) - timeNow).TotalSeconds);
		}
		
		[Category("Time")]
		public void Add1Minute()
		{
			var timeManipulator = MainInstaller.Resolve<IGameServices>().TimeService as ITimeManipulator;
			var timeNow = timeManipulator.DateTimeUtcNow;
			
			timeManipulator.AddTime((float) (timeNow.AddHours(1) - timeNow).TotalSeconds);
		}
	}
}