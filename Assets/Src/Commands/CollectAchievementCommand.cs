using Ids;
using Logic;

namespace Commands
{
	/// <inheritdoc cref="IGameCommand" />
	public struct CollectAchievementCommand : IGameCommand
	{
		public UniqueId Achievement;
		
		/// <inheritdoc />
		public void Execute(IGameLogic gameLogic)
		{
			gameLogic.AchievementLogic.CollectAchievement(Achievement);
		}
	}
}