using System.Collections.Generic;
using System.Collections.ObjectModel;

/* AUTO GENERATED CODE */
namespace Ids
{
	public enum GameId
	{
		MainCamera,
		InputSystem,
		Random,
		MainCurrency,
		SoftCurrency,
		HardCurrency,
		AppleTree,
		ChristmasTree,
		NormalTree,
		Squirrel,
		Blackbird,
		Fruitbat
	}

	public enum GameIdGroup
	{
		WorldReference,
		GameDesign,
		Currency,
		Tree,
		Card,
		Animal
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
					GameId.MainCamera, new List<GameIdGroup>
					{
						GameIdGroup.WorldReference
					}.AsReadOnly()
				},
				{
					GameId.InputSystem, new List<GameIdGroup>
					{
						GameIdGroup.WorldReference
					}.AsReadOnly()
				},
				{
					GameId.Random, new List<GameIdGroup>
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
						GameIdGroup.Tree,
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.ChristmasTree, new List<GameIdGroup>
					{
						GameIdGroup.Tree,
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.NormalTree, new List<GameIdGroup>
					{
						GameIdGroup.Tree,
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.Squirrel, new List<GameIdGroup>
					{
						GameIdGroup.Animal,
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.Blackbird, new List<GameIdGroup>
					{
						GameIdGroup.Animal,
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.Fruitbat, new List<GameIdGroup>
					{
						GameIdGroup.Animal,
						GameIdGroup.Card
					}.AsReadOnly()
				},
			};

		private static readonly Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> _ids =
			new Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> (new GameIdGroupComparer())
			{
				{
					GameIdGroup.WorldReference, new List<GameId>
					{
						GameId.MainCamera,
						GameId.InputSystem
					}.AsReadOnly()
				},
				{
					GameIdGroup.GameDesign, new List<GameId>
					{
						GameId.Random
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
					GameIdGroup.Card, new List<GameId>
					{
						GameId.AppleTree,
						GameId.ChristmasTree,
						GameId.NormalTree,
						GameId.Squirrel,
						GameId.Blackbird,
						GameId.Fruitbat
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
			};
	}
}
