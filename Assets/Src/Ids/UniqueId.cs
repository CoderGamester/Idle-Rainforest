using System;
using System.Collections.Generic;
using GameLovers;

namespace Ids
{
	/// <summary>
	/// Used to reference any entity by an unique Id value
	/// </summary>
	[Serializable]
	public struct UniqueId : IEquatable<UniqueId>, IComparable<UniqueId>, IComparable<ulong>
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
		public int CompareTo(UniqueId value)
		{
			if (Id < value.Id)
			{
				return -1;
			}
			
			return Id > value.Id ? 1 : 0;
		}

		/// <inheritdoc />
		public int CompareTo(ulong value)
		{
			if (Id < value)
			{
				return -1;
			}
			
			return Id > value ? 1 : 0;
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
			return $"UniqueId: {Id.ToString()}";
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

	/// <inheritdoc />
	/// <remarks>
	/// Enhances the <see cref="IIdList"/> with a defined <see cref="UniqueId"/> key type
	/// </remarks>
	public interface IUniqueIdList : IIdList
	{
	}
	
	/// <inheritdoc cref="IUniqueIdList" />
	public interface IUniqueIdListReader<T> : IIdListReader<UniqueId, T>, IUniqueIdList
		where T : struct
	{
	}

	/// <inheritdoc cref="IUniqueIdList" />
	public interface IUniqueIdList<T> : IIdList<UniqueId, T>, IUniqueIdListReader<T>
		where T : struct
	{
	}

	/// <inheritdoc cref="IUniqueIdList" />
	public class UniqueIdList<T> : IdList<UniqueId, T>, IUniqueIdList<T> 
		where T : struct
	{
		public UniqueIdList(Func<T, UniqueId> referenceIdResolver, IList<T> list) : base(referenceIdResolver, list)
		{
		}
	}
}