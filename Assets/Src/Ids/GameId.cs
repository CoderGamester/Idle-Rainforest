using System.Collections.Generic;
using System.Collections.ObjectModel;

/* AUTO GENERATED CODE */
namespace Ids
{
	public enum GameId
	{
		Seeds,
		Fruits,
		Acorns,
		BuildingZoo,
		BuildingPark,
		BuildingAquarium
	}

	public enum GameIdGroup
	{
		UserResource,
		Building
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
					GameId.Seeds, new List<GameIdGroup>
					{
						GameIdGroup.UserResource
					}.AsReadOnly()
				},
				{
					GameId.Fruits, new List<GameIdGroup>
					{
						GameIdGroup.UserResource
					}.AsReadOnly()
				},
				{
					GameId.Acorns, new List<GameIdGroup>
					{
						GameIdGroup.UserResource
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
			};

		private static readonly Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> _ids =
			new Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> (new GameIdGroupComparer())
			{
				{
					GameIdGroup.UserResource, new List<GameId>
					{
						GameId.Seeds,
						GameId.Fruits,
						GameId.Acorns
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
			};
	}
}
