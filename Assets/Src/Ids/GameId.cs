using System.Collections.Generic;
using System.Collections.ObjectModel;

/* AUTO GENERATED CODE */
namespace Ids
{
	public enum GameId
	{
		WorldCoins,
		WoodCollector,
		IronMine,
		FabricFactory,
		FoodFarm,
		Wood,
		Iron,
		Fabric,
		Food
	}

	public enum GameIdGroup
	{
		UserResource,
		Building,
		Resource
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
					GameId.WorldCoins, new List<GameIdGroup>
					{
						GameIdGroup.UserResource
					}.AsReadOnly()
				},
				{
					GameId.WoodCollector, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.IronMine, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.FabricFactory, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.FoodFarm, new List<GameIdGroup>
					{
						GameIdGroup.Building
					}.AsReadOnly()
				},
				{
					GameId.Wood, new List<GameIdGroup>
					{
						GameIdGroup.Resource
					}.AsReadOnly()
				},
				{
					GameId.Iron, new List<GameIdGroup>
					{
						GameIdGroup.Resource
					}.AsReadOnly()
				},
				{
					GameId.Fabric, new List<GameIdGroup>
					{
						GameIdGroup.Resource
					}.AsReadOnly()
				},
				{
					GameId.Food, new List<GameIdGroup>
					{
						GameIdGroup.Resource
					}.AsReadOnly()
				},
			};

		private static readonly Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> _ids =
			new Dictionary<GameIdGroup, ReadOnlyCollection<GameId>> (new GameIdGroupComparer())
			{
				{
					GameIdGroup.UserResource, new List<GameId>
					{
						GameId.WorldCoins
					}.AsReadOnly()
				},
				{
					GameIdGroup.Building, new List<GameId>
					{
						GameId.WoodCollector,
						GameId.IronMine,
						GameId.FabricFactory,
						GameId.FoodFarm
					}.AsReadOnly()
				},
				{
					GameIdGroup.Resource, new List<GameId>
					{
						GameId.Wood,
						GameId.Iron,
						GameId.Fabric,
						GameId.Food
					}.AsReadOnly()
				},
			};
	}
}
