using System;
using System.Collections;
using System.Collections.Generic;

namespace Ids
{
	/// <summary>
	/// Used to reference any entity by an unique Id value
	/// </summary>
	[Serializable]
	public struct EntityId : IEquatable<EntityId>
	{
		public static readonly EntityId Invalid = new EntityId(0);

		public readonly ulong Id;

		public bool IsValid => this != Invalid;

		public EntityId(ulong id)
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
 
			return obj is EntityId && Equals((EntityId)obj);
		}
 
		public bool Equals(EntityId other)
		{
			return Id == other.Id;
		}
 
		public static bool operator ==(EntityId p1, EntityId p2)
		{
			return p1.Id == p2.Id;
		}
 
		public static bool operator !=(EntityId p1, EntityId p2)
		{
			return p1.Id != p2.Id;
		}
		
		public static implicit operator ulong(EntityId id)
		{
			return id.Id;
		}
		
		public static implicit operator EntityId(ulong id)
		{
			return new EntityId(id);
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
	public struct UniqueIdKeyComparer : IEqualityComparer<EntityId>
	{
		/// <inheritdoc />
		public bool Equals(EntityId x, EntityId y)
		{
			return x.Id == y.Id;
		}

		/// <inheritdoc />
		public int GetHashCode(EntityId obj)
		{
			return (int) obj.Id;
		}
	}

	/// <summary>
	/// Enhances the <see cref="Dictionary{TKey,TValue}"/> to be an <see cref="EntityId"/> Key reference <see cref="System.Collections.IDictionary"/>
	/// </summary>
	public interface IEntityDictionary : IDictionary
	{
		/// <summary>
		/// Checks if the dictionary has or not the given <paramref name="entityId"/>
		/// </summary>
		bool ContainsEntity(EntityId entityId);
		
		/// <summary>
		/// Removes the entry of the dictionary with the given <paramref name="entityId"/> key
		/// </summary>
		void RemoveEntity(EntityId entityId);
	}

	/// <inheritdoc cref="IEntityDictionary" />
	public class EntityDictionary<TValue> : Dictionary<EntityId, TValue>, IEntityDictionary
	{
		/// <inheritdoc />
		public bool ContainsEntity(EntityId entityId)
		{
			return ContainsKey(entityId);
		}

		/// <inheritdoc />
		public void RemoveEntity(EntityId entityId)
		{
			Remove(entityId);
		}
	}
}