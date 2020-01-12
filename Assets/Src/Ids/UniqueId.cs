using System;
using System.Collections.Generic;

namespace Ids
{
	/// <summary>
	/// Used to reference any entity by an unique Id value
	/// </summary>
	[Serializable]
	public struct UniqueId : IEquatable<UniqueId>
	{
		public static readonly UniqueId Invalid = new UniqueId(0);

		public readonly ulong Id;

		public bool IsValid => this != Invalid;

		public UniqueId(ulong id)
		{
			Id = id;
		}
 
		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (int)Id;
		}
 
		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
 
			return obj is UniqueId && Equals((UniqueId)obj);
		}
 
		public bool Equals(UniqueId other)
		{
			return Id == other.Id;
		}
 
		public static bool operator ==(UniqueId p1, UniqueId p2)
		{
			return p1.Id == p2.Id;
		}
 
		public static bool operator !=(UniqueId p1, UniqueId p2)
		{
			return p1.Id != p2.Id;
		}
		
		public static implicit operator ulong(UniqueId id)
		{
			return id.Id;
		}
		
		public static implicit operator UniqueId(ulong id)
		{
			return new UniqueId(id);
		}
 
		/// <inheritdoc />
		public override string ToString()
		{
			return $"ProcessId: {Id.ToString()}";
		}
	}

	/// <summary>
	/// Avoids boxing for Dictionary
	/// </summary>
	public struct UniqueIdKeyComparer : IEqualityComparer<UniqueId>
	{
		/// <inheritdoc />
		public bool Equals(UniqueId x, UniqueId y)
		{
			return x.Id == y.Id;
		}

		/// <inheritdoc />
		public int GetHashCode(UniqueId obj)
		{
			return (int) obj.Id;
		}
	}

	/// <inheritdoc cref="IUniqueIdList" />
	/// <remarks>
	/// Read only uniqueId list interface
	/// </remarks>
	public interface IUniqueIdListReader
	{
		/// <summary>
		/// TODO:
		/// </summary>
		bool Contains(UniqueId id);
	}

	/// <summary>
	/// TODO:
	/// </summary>
	public interface IUniqueIdList : IUniqueIdListReader
	{
		/// <summary>
		/// Removes the data associated with the given <paramref name="id"/>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown if there is no data associated with the given <paramref name="id"/>
		/// </exception>
		void Remove(UniqueId id);
 
		/// <summary>
		/// Removes the data associated with the given <paramref name="id"/> if present in the list
		/// </summary>
		void TryRemove(UniqueId id);
	}
	
	/// <inheritdoc />
	public interface IUniqueIdListReader<T> : IUniqueIdListReader
		where T : struct
	{
		/// <summary>
		/// Looks up and return the <typeparamref name="T"/> data that is associated with the given <paramref name="id"/>.
		/// It return null when no data is associated with the given <paramref name="id"/>
		/// </summary>
		T? TryGet(UniqueId id);
 
		/// <summary>
		/// Looks up and return the <typeparamref name="T"/> data that is associated with the given <paramref name="id"/>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown when there is no data is associated with the given <paramref name="id"/>
		/// </exception>
		T Get(UniqueId id);
		
		/// <summary>
		/// TODO:
		/// </summary>
		List<T> GetList();

		/// <summary>
		/// TODO:
		/// </summary>
		void Observe(UniqueId uniqueId, ListUpdateType updateType, Action<T> onUpdate);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void StopObserving(UniqueId uniqueId, ListUpdateType updateType, Action<T> onUpdate);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void StopObserving(UniqueId uniqueId);
	}

	/// <inheritdoc cref="IUniqueIdList" />
	public interface IUniqueIdList<T> : IUniqueIdListReader<T>, IUniqueIdList
		where T : struct
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void Add(T data);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void Set(T data);
	}

	public enum ListUpdateType
	{
		Added,
		Updated,
		Removed
	}
 
	/// <summary>
	/// TODO:
	/// </summary>
	public class UniqueIdList<T> : IUniqueIdList<T> where T : struct
	{
		private readonly Func<List<T>> _persistentListResolver;
		private readonly Func<T, UniqueId> _referenceIdResolver;
		private readonly IDictionary<UniqueId, IList<Action<T>>> _onAddActions = new Dictionary<UniqueId, IList<Action<T>>>();
		private readonly IDictionary<UniqueId, IList<Action<T>>> _onUpdateActions = new Dictionary<UniqueId, IList<Action<T>>>();
		private readonly IDictionary<UniqueId, IList<Action<T>>> _onRemoveActions = new Dictionary<UniqueId, IList<Action<T>>>();
 
		public UniqueIdList(Func<T, UniqueId> referenceIdResolver, Func<List<T>> persistentListResolver)
		{
			_persistentListResolver = persistentListResolver;
			_referenceIdResolver = referenceIdResolver;
		}
 
		/// <inheritdoc />
		public bool Contains(UniqueId id)
		{
			return FindIndex(id) >= 0;
		}

		/// <inheritdoc />
		public T? TryGet(UniqueId id)
		{
			int index = FindIndex(id);
			if (index < 0)
			{
				return null;
			}
 
			return _persistentListResolver()[index];
		}
 
		/// <inheritdoc />
		public T Get(UniqueId id)
		{
			var data = TryGet(id);
			if (data.HasValue)
			{
				return data.Value;
			}
 
			throw new ArgumentException($"Can not find {typeof(T).Name} to id {id.Id.ToString()}");
		}
 
		/// <inheritdoc />
		public List<T> GetList()
		{
			return _persistentListResolver();
		}

		/// <inheritdoc />
		public void Observe(UniqueId uniqueId, ListUpdateType updateType, Action<T> onUpdate)
		{
			switch (updateType)
			{
				case ListUpdateType.Added:
					if (!_onAddActions.TryGetValue(uniqueId, out var addList))
					{
						addList = new List<Action<T>>();
						
						_onAddActions.Add(uniqueId, addList);
					}
					
					addList.Add(onUpdate);
					break;
				case ListUpdateType.Updated:
					if (!_onUpdateActions.TryGetValue(uniqueId, out var updateList))
					{
						updateList = new List<Action<T>>();
						
						_onUpdateActions.Add(uniqueId, updateList);
					}
					
					updateList.Add(onUpdate);
					break;
				case ListUpdateType.Removed:
					if (!_onRemoveActions.TryGetValue(uniqueId, out var removeList))
					{
						removeList = new List<Action<T>>();
						
						_onRemoveActions.Add(uniqueId, removeList);
					}
					
					removeList.Add(onUpdate);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(updateType), updateType, "Wrong update type");
			}
		}

		/// <inheritdoc />
		public void StopObserving(UniqueId uniqueId, ListUpdateType updateType, Action<T> onUpdate)
		{
			switch (updateType)
			{
				case ListUpdateType.Added:
					if (_onAddActions.TryGetValue(uniqueId, out var addList))
					{
						addList.Remove(onUpdate);
					}
					break;
				case ListUpdateType.Updated:
					if (_onUpdateActions.TryGetValue(uniqueId, out var updateList))
					{
						updateList.Remove(onUpdate);
					}
					break;
				case ListUpdateType.Removed:
					if (_onRemoveActions.TryGetValue(uniqueId, out var removeList))
					{
						removeList.Remove(onUpdate);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(updateType), updateType, "Wrong update type");
			}
		}

		public void StopObserving(UniqueId uniqueId)
		{
			if (_onAddActions.TryGetValue(uniqueId, out var addList))
			{
				addList.Clear();

				_onAddActions.Remove(uniqueId);
			}
			if (_onUpdateActions.TryGetValue(uniqueId, out var updateList))
			{
				updateList.Clear();

				_onUpdateActions.Remove(uniqueId);
			}
			if (_onRemoveActions.TryGetValue(uniqueId, out var removeList))
			{
				removeList.Clear();

				_onRemoveActions.Remove(uniqueId);
			}
		}

		/// <inheritdoc />
		public void Add(T data)
		{
			var id = _referenceIdResolver(data);
			if (FindIndex(id) >= 0)
			{
				throw new ArgumentException($"Cannot add {nameof(T)} with uniqueId {id.ToString()}, because it already exists");
			}
 
			_persistentListResolver().Add(data);
		}
 
		/// <inheritdoc />
		public void Remove(UniqueId id)
		{
			int index = FindIndex(id);
			if (index < 0)
			{
				throw new ArgumentException($"Cannot remove {nameof(T)} with uniqueId {id.ToString()}, because it does not exists");
			}
 
			_persistentListResolver().RemoveAt(index);
		}
 
		/// <inheritdoc />
		public void TryRemove(UniqueId id)
		{
			var data = TryGet(id);
			if (data.HasValue)
			{
				Remove(id);
			}
		}

		/// <inheritdoc />
		public void Set(T data)
		{
			var id = _referenceIdResolver(data);
			int index = FindIndex(id);
			if (index < 0)
			{
				throw new ArgumentException($"Could not find: {nameof(T)} with uniqueId {id.ToString()}");
			}
 
			_persistentListResolver()[index] = data;
		}
 
		private int FindIndex(UniqueId id)
		{
			var list = _persistentListResolver();
			var comparer = new UniqueIdKeyComparer();
			for (var i = 0; i < list.Count; i++)
			{
				if (comparer.Equals(_referenceIdResolver(list[i]), id))
				{
					return i;
				}
			}
 
			return -1;
		}
	}
}