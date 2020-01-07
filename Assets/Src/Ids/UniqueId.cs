using System;
using System.Collections.Generic;

namespace Ids
{
	/// <summary>
	/// Used to reference any <seealso cref="Unity.Entities.Entity"/> by an Unique Id value
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
		public bool Equals(UniqueId x, UniqueId y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(UniqueId obj)
		{
			return (int) obj.Id;
		}
	}
}