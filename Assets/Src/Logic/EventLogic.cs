using System;
using Infos;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IEventDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		EventInfo GetEventInfo();
	}

	/// <inheritdoc />
	public interface IEventLogic : IEventDataProvider
	{
		
	}
	
	/// <inheritdoc />
	public class EventLogic : IEventLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		
		private EventLogic() {}

		public EventLogic(IGameInternalLogic gameLogic)
		{
			_gameLogic = gameLogic;
		}


		/// <inheritdoc />
		public EventInfo GetEventInfo()
		{
			var startTime = _gameLogic.TimeService.DateTimeUtcNow.Date;
			var endTime = startTime.AddDays(1);
			var isRunning = _gameLogic.TimeService.DateTimeUtcNow > startTime && _gameLogic.TimeService.DateTimeUtcNow < endTime;
			
			return new EventInfo
			{
				StartTime = startTime,
				EndTime = endTime,
				IsRunning = isRunning,
				ShowEventPopUp = isRunning && (_gameLogic.IsFirstSession || _gameLogic.DataProviderInternalLogic.AppData.LastLoginTime < startTime)
			};
		}
	}
}