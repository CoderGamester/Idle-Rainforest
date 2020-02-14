using System.Collections.Generic;
using System.Collections.ObjectModel;

/* AUTO GENERATED CODE */
namespace Ids
{
	public enum GameId
	{
		Random,
		Time,
		MainCurrency,
		SoftCurrency,
		HardCurrency,
		AppleTree,
		ChristmasTree,
		NormalTree,
		Squirrel,
		Blackbird,
		Fruitbat,
		CollectMainCurrency,
		CollectSoftCurrency,
		UpgradeAnimal,
		UpgradeLevelTree,
		AutomateTree
	}

	public enum GameIdGroup
	{
		GameDesign,
		Currency,
		Tree,
		Animal,
		Achievement
	}

	public static class GameIdLookup
	{
		public static bool IsInGroup(this GameId id, GameIdGroup group)
		{
			if (!_groups.TryGetValue(id, out var groups))
			{
				return false;
			}
			return groups.Contains(group);
		}

		public static IList<GameId> GetIds(this GameIdGroup group)
		{
			return _ids[group];
		}

		public static IList<GameIdGroup> GetGroups(this GameId id)
		{
			return _groups[id];
		}

		public class GameIdComparer : IEqualityComparer<GameId>
		{
			public bool Equals(GameId x, GameId y)
			{
				return x == y;
			}

			public int GetHashCode(GameId obj)
			{
				return (int)obj;
			}
		}

		public class GameIdGroupComparer : IEqualityComparer<GameIdGroup>
		{
			public bool Equals(GameIdGroup x, GameIdGroup y)
			{
				return x == y;
			}

			public int GetHashCode(GameIdGroup obj)
			{
				return (int)obj;
			}
		}

		private static readonly Dictionary<GameId, ReadOnlyCollection<GameIdGroup>> _groups =
			new Dictionary<GameId, ReadOnlyCollection<GameIdGroup>> (new GameIdComparer())
			{
				{
					GameId.Random, new List<GameIdGroup>
					{
						GameIdGroup.GameDesign
					}.AsReadOnly()
				},
				{
					GameId.Time, new List<GameIdGroup>
					{
						GameIdGroup.GameDesign
					}.AsReadOnly()
				},
				{
					GameId.MainCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Currency
					}.AsReadOnly()
				},
				{
					GameId.SoftCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Currency
					}.AsReadOnly()
				},
				{
					GameId.HardCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Currency
					}.AsReadOnly()
				},
				{
					GameId.AppleTree, new List<GameIdGroup>
					{
						GameIdGroup.Tree
					}.AsReadOnly()
				},
				{
					GameId.ChristmasTree, new List<GameIdGroup>
					{
						GameIdGroup.Tree
					}.AsReadOnly()
				},
				{
					GameId.NormalTree, new List<GameIdGroup>
					{
						GameIdGroup.Tree
					}.AsReadOnly()
				},
				{
					GameId.Squirrel, new List<GameIdGroup>
					{
						GameIdGroup.Animal
					}.AsReadOnly()
				},
				{
					GameId.Blackbird, new List<GameIdGroup>
					{
						GameIdGroup.Animal
					}.AsReadOnly()
				},
				{
					GameId.Fruitbat, new List<GameIdGroup>
					{
						GameIdGroup.Animal
					}.AsReadOnly()
				},
				{
					GameId.CollectMainCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Achievement
					}.AsReadOnly()
				},
				{
					GameId.CollectSoftCurrency, new List<GameIdGroup>
					{
						GameIdGroup.Achievement
					}.AsReadOnly()
				},
				{
					GameId.UpgradeAnimal, new List<GameIdGroup>
					{
						GameIdGroup.Achievement
					}.AsReadOnly()
				},
				{
					GameId.UpgradeLevelTree, new List<GameIdGroup>
					{
						GameIdGroup.Achievement
					}.AsReadOnly()
				},
				{
					GameId.AutomateTree, new List<GameIdGroup>
					{
						GameIdGroup.Achievement
					}.AsReadOnly()
				},
			};

		private static readonly Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> _ids =
			new Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> (new GameIdGroupComparer())
			{
				{
					GameIdGroup.GameDesign, new List<GameId>
					{
						GameId.Random,
						GameId.Time
					}.AsReadOnly()
				},
				{
					GameIdGroup.Currency, new List<GameId>
					{
						GameId.MainCurrency,
						GameId.SoftCurrency,
						GameId.HardCurrency
					}.AsReadOnly()
				},
				{
					GameIdGroup.Tree, new List<GameId>
					{
						GameId.AppleTree,
						GameId.ChristmasTree,
						GameId.NormalTree
					}.AsReadOnly()
				},
				{
					GameIdGroup.Animal, new List<GameId>
					{
						GameId.Squirrel,
						GameId.Blackbird,
						GameId.Fruitbat
					}.AsReadOnly()
				},
				{
					GameIdGroup.Achievement, new List<GameId>
					{
						GameId.CollectMainCurrency,
						GameId.CollectSoftCurrency,
						GameId.UpgradeAnimal,
						GameId.UpgradeLevelTree,
						GameId.AutomateTree
					}.AsReadOnly()
				},
			};
	}
}
