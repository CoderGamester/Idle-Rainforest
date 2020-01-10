using System.Collections.Generic;
using Ids;

namespace Logic
{
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IGameIdDataProvider
	{
		/// <summary>
		/// Requests the <see cref="GameId"/> of the given <paramref name="entity"/>
		/// </summary>
		GameId GetGameId(EntityId entity);

		/// <summary>
		/// TODO:
		/// </summary>
		bool IsInGroup(EntityId entity, GameIdGroup group);
	}

	/// <inheritdoc />
	public interface IGameIdLogic : IGameIdDataProvider
	{
	}
	
	/// <inheritdoc />
	public class GameIdLogic : IGameIdLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IDictionary<EntityId, GameId> _data;
		
		private GameIdLogic() {}

		public GameIdLogic(IGameInternalLogic gameLogic, IDictionary<EntityId, GameId> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}

		/// <inheritdoc />
		public GameId GetGameId(EntityId entity)
		{
			return _data[entity];
		}

		/// <inheritdoc />
		public bool IsInGroup(EntityId entity, GameIdGroup group)
		{
			return GetGameId(entity).IsInGroup(group);
		}
	}
}