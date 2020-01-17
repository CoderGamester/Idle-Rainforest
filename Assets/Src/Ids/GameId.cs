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
		BuildingZoo,
		BuildingPark,
		BuildingAquarium,
		Animal1,
		Animal2,
		Animal3
	}

	public enum GameIdGroup
	{
		GameDesign,
		Currency,
		Building,
		Card
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
					GameId.BuildingZoo, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.BuildingPark, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.BuildingAquarium, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.Animal1, new List<GameIdGroup>
					{
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.Animal2, new List<GameIdGroup>
					{
						GameIdGroup.Card
					}.AsReadOnly()
				},
				{
					GameId.Animal3, new List<GameIdGroup>
					{
						GameIdGroup.Card
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
					GameIdGroup.Building, new List<GameId>
					{
						GameId.BuildingZoo,
						GameId.BuildingPark,
						GameId.BuildingAquarium
					}.AsReadOnly()
				},
				{
					GameIdGroup.Card, new List<GameId>
					{
						GameId.Animal1,
						GameId.Animal2,
						GameId.Animal3
					}.AsReadOnly()
				},
			};
	}
}
