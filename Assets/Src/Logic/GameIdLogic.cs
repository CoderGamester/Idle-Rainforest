using Data;
using Ids;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IGameIdDataProvider
	{
		/// <summary>
		/// TODO:
		/// </summary>
		IUniqueIdListReader<GameIdData> Data { get; }
	}

	/// <inheritdoc />
	public interface IGameIdLogic : IGameIdDataProvider
	{
	}
	
	/// <inheritdoc />
	public class GameIdLogic : IGameIdLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IUniqueIdList<GameIdData> _data;

		/// <inheritdoc />
		public IUniqueIdListReader<GameIdData> Data => _data;
		
		private GameIdLogic() {}

		public GameIdLogic(IGameInternalLogic gameLogic, IUniqueIdList<GameIdData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}
	}
}