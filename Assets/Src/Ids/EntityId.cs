using System;
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
 
		public override int GetHashCode()
		{
			return (int)Id;
		}
 
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
		public bool Equals(EntityId x, EntityId y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(EntityId obj)
		{
			return (int) obj.Id;
		}
	}
}