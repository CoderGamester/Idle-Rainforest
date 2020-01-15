using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public enum ListUpdateType
	{
		Added,
		Updated,
		Removed
	}
	
	/// <summary>
	/// TODO:
	/// </summary>
	public interface IIdList
	{
	}

	/// <inheritdoc />
	/// <remarks>
	/// Read only uniqueId list interface
	/// </remarks>
	public interface IIdListReader<in TKey, TValue> : IIdList 
		where TValue : struct
	{
		/// <summary>
		/// Looks up the data that is associated with the given <paramref name="id"/>.
		/// It return true if is able to delivery out the given <paramref name="value"/>.
		/// The <paramref name="value"/> will be the default if returns false
		/// </summary>
		bool TryGet(TKey id, out TValue value);
 
		/// <summary>
		/// Looks up and return the ata that is associated with the given <paramref name="id"/>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown when there is no data is associated with the given <paramref name="id"/>
		/// </exception>
		TValue Get(TKey id);
		
		/// <summary>
		/// TODO:
		/// </summary>
		IReadOnlyList<TValue> GetReadOnlyList();

		/// <summary>
		/// TODO:
		/// </summary>
		void Observe(TKey id, ListUpdateType updateType, Action<TValue> onUpdate);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void StopObserving(TKey id, ListUpdateType updateType, Action<TValue> onUpdate);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void StopObserving(TKey id);
	}

	/// <inheritdoc cref="IIdList" />
	public interface IIdList<in TKey, TValue> : IIdListReader<TKey, TValue>
		where TValue : struct
	{
		/// <summary>
		/// TODO:
		/// </summary>
		void Add(TValue data);
		
		/// <summary>
		/// TODO:
		/// </summary>
		void Set(TValue data);

		/// <summary>
		/// TODO:
		/// </summary>
		List<TValue> GetList();
		
		/// <summary>
		/// Removes the data associated with the given <paramref name="id"/>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown if there is no data associated with the given <paramref name="id"/>
		/// </exception>
		void Remove(TKey id);
		
		/// <summary>
		/// Removes the the given <paramref name="data"/>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Thrown if there is no <paramref name="data"/> in this list
		/// </exception>
		void RemoveData(TValue data);
 
		/// <summary>
		/// Removes the data associated with the given <paramref name="id"/> if present in the list
		/// </summary>
		void TryRemove(TKey id);
 
		/// <summary>
		/// Removes the <paramref name="data"/> if present in the list
		/// </summary>
		void TryRemoveData(TValue data);
	}
 
	/// <summary>
	/// TODO:
	/// </summary>
	public class IdList<TKey, TValue> : IIdList<TKey, TValue>
		where TValue : struct
	{
		private readonly Func<TValue, TKey> _referenceIdResolver;
		private readonly Func<List<TValue>> _persistentListResolver;
		private readonly EqualityComparer<TKey> _comparer = EqualityComparer<TKey>.Default;
		private readonly IDictionary<TKey, IList<Action<TValue>>> _onAddActions = new Dictionary<TKey, IList<Action<TValue>>>();
		private readonly IDictionary<TKey, IList<Action<TValue>>> _onUpdateActions = new Dictionary<TKey, IList<Action<TValue>>>();
		private readonly IDictionary<TKey, IList<Action<TValue>>> _onRemoveActions = new Dictionary<TKey, IList<Action<TValue>>>();
		
		private IdList() {}
 
		public IdList(Func<TValue, TKey> referenceIdResolver, Func<List<TValue>> persistentListResolver)
		{
			_persistentListResolver = persistentListResolver;
			_referenceIdResolver = referenceIdResolver;
		}

		/// <inheritdoc />
		public bool TryGet(TKey id, out TValue value)
		{
			int index = FindIndex(id);
			if (index < 0)
			{
				value = default;
				
				return false;
			}
 
			value =  _persistentListResolver()[index];

			return true;
		}
 
		/// <inheritdoc />
		public TValue Get(TKey id)
		{
			if (TryGet(id, out TValue data))
			{
				return data;
			}

			throw new ArgumentException($"Can not find {typeof(TValue).Name} to id {id.ToString()}");
		}

		/// <inheritdoc />
		public IReadOnlyList<TValue> GetReadOnlyList()
		{
			return _persistentListResolver().AsReadOnly();
		}

		/// <inheritdoc />
		public void Observe(TKey id, ListUpdateType updateType, Action<TValue> onUpdate)
		{
			switch (updateType)
			{
				case ListUpdateType.Added:
					if (!_onAddActions.TryGetValue(id, out var addList))
					{
						addList = new List<Action<TValue>>();
						
						_onAddActions.Add(id, addList);
					}
					
					addList.Add(onUpdate);
					break;
				case ListUpdateType.Updated:
					if (!_onUpdateActions.TryGetValue(id, out var updateList))
					{
						updateList = new List<Action<TValue>>();
						
						_onUpdateActions.Add(id, updateList);
					}
					
					updateList.Add(onUpdate);
					break;
				case ListUpdateType.Removed:
					if (!_onRemoveActions.TryGetValue(id, out var removeList))
					{
						removeList = new List<Action<TValue>>();
						
						_onRemoveActions.Add(id, removeList);
					}
					
					removeList.Add(onUpdate);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(updateType), updateType, "Wrong update type");
			}
		}

		/// <inheritdoc />
		public void StopObserving(TKey id, ListUpdateType updateType, Action<TValue> onUpdate)
		{
			switch (updateType)
			{
				case ListUpdateType.Added:
					if (_onAddActions.TryGetValue(id, out var addList))
					{
						addList.Remove(onUpdate);
					}
					break;
				case ListUpdateType.Updated:
					if (_onUpdateActions.TryGetValue(id, out var updateList))
					{
						updateList.Remove(onUpdate);
					}
					break;
				case ListUpdateType.Removed:
					if (_onRemoveActions.TryGetValue(id, out var removeList))
					{
						removeList.Remove(onUpdate);
					}
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(updateType), updateType, "Wrong update type");
			}
		}

		public void StopObserving(TKey id)
		{
			if (_onAddActions.TryGetValue(id, out var addList))
			{
				addList.Clear();

				_onAddActions.Remove(id);
			}
			if (_onUpdateActions.TryGetValue(id, out var updateList))
			{
				updateList.Clear();

				_onUpdateActions.Remove(id);
			}
			if (_onRemoveActions.TryGetValue(id, out var removeList))
			{
				removeList.Clear();

				_onRemoveActions.Remove(id);
			}
		}

		/// <inheritdoc />
		public void Add(TValue data)
		{			
			var id = _referenceIdResolver(data);
			if (FindIndex(id) >= 0)
			{
				throw new ArgumentException($"Cannot add {nameof(TValue)} with id {id.ToString()}, because it already exists");
			}
 
			_persistentListResolver().Add(data);
			
			if (_onAddActions.TryGetValue(id, out var actions))
			{
				for (var i = 0; i < actions.Count; i++)
				{
					actions[i](data);
				}
			}
		}
 
		/// <inheritdoc />
		public List<TValue> GetList()
		{
			return _persistentListResolver();
		}

		/// <inheritdoc />
		public void Remove(TKey id)
		{
			int index = FindIndex(id);
			if (index < 0)
			{
				throw new ArgumentException($"Cannot remove {nameof(TValue)} with id {id.ToString()}, because it does not exists");
			}

			Remove(index, id);
		}

		/// <inheritdoc />
		public void RemoveData(TValue data)
		{
			Remove(_referenceIdResolver(data));
		}

		/// <inheritdoc />
		public void TryRemove(TKey id)
		{
			int index = FindIndex(id);
			if (index >= 0)
			{
				Remove(index, id);
			}
		}

		/// <inheritdoc />
		public void TryRemoveData(TValue data)
		{
			TryRemove(_referenceIdResolver(data));
		}

		/// <inheritdoc />
		public void Set(TValue data)
		{			
			var id = _referenceIdResolver(data);
			int index = FindIndex(id);
			if (index < 0)
			{
				throw new ArgumentException($"Could not find: {nameof(TValue)} with id {id.ToString()}");
			}
 
			_persistentListResolver()[index] = data;
			
			if (_onUpdateActions.TryGetValue(id, out var actions))
			{
				for (var i = 0; i < actions.Count; i++)
				{
					actions[i](data);
				}
			}
		}
		
		private int FindIndex(TKey id)
		{
			var list = _persistentListResolver();
			for (var i = 0; i < list.Count; i++)
			{
				if (_comparer.Equals(_referenceIdResolver(list[i]), id))
				{
					return i;
				}
			}
 
			return -1;
		}

		private void Remove(int index, TKey id)
		{
			if (_onRemoveActions.TryGetValue(id, out var actions))
			{
				var data = _persistentListResolver()[index];
				
				for (var i = 0; i < actions.Count; i++)
				{
					actions[i](data);
				}
			}
			
			_persistentListResolver().RemoveAt(index);
		}
	}
}