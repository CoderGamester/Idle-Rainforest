using Data;
using Ids;
using TMPro;

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

		/// <summary>
		/// Requests the <see cref="GameIdData"/> for the first element found with the given <paramref name="gameId"/>
		/// </summary>
		/// <exception cref="LogicException">
		/// Thrown if there is no element with the given <paramref name="gameId"/>
		/// </exception>
		GameIdData GetData(GameId gameId);

		/// <summary>
		/// Requests the <see cref="GameIdData"/> for the first element found with the given <paramref name="gameId"/>
		/// Returns true if the element was found
		/// </summary>
		bool TryGetData(GameId gameId, out GameIdData data);
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

		/// <inheritdoc />
		public GameIdData GetData(GameId gameId)
		{
			if (TryGetData(gameId, out var data))
			{
				return data;
			}

			throw new LogicException($"There is no data for the {gameId}");
		}

		/// <inheritdoc />
		public bool TryGetData(GameId gameId, out GameIdData data)
		{
			var list = _data.GetList();

			for (var i = 0; i < list.Count; i++)
			{
				if (list[i].GameId == gameId)
				{
					data = list[i];

					return true;
				}
			}

			data = default;
			
			return false;
		}
	}
}