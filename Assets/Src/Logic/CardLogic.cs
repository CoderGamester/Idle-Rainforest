using Data;
using Ids;
using Utils;

namespace Logic
{
	public interface ICardDataProvider
	{
		
	}

	/// <inheritdoc />
	public interface ICardLogic : ICardDataProvider
	{
		
	}
	
	/// <inheritdoc />
	public class CardLogic : ICardLogic
	{
		private readonly IGameInternalLogic _gameLogic;
		private readonly IIdList<GameId, CardData> _data;
		
		private CardLogic() {}

		public CardLogic(IGameInternalLogic gameLogic, IIdList<GameId, CardData> data)
		{
			_gameLogic = gameLogic;
			_data = data;
		}
	}
}