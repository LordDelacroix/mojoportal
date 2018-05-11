using System;
using System.Collections.Generic;
using System.Linq;

namespace ironclad.Features.UI.Tools
{
	/// <summary>list that can be accessed as list of parent type to fix some COVARIANCE / CONTRAVARIANCE
	/// problems dealing with lists of objects that have a common base</summary>
	/// <remarks>Bagnes, Aug 2009.</remarks>
	public class List<T, ParentT> : List<T>, IList<ParentT> where T : class, ParentT where ParentT : class
	{
		private ListViewer<T> listViewer;

		/// <summary>
		/// Initializes a new instance of the class that is empty and has the default initial capacity.
		/// </summary>
		public List() { }

		/// <summary>
		/// Initializes a new instance of the class that contains elements copied from the specified collection
		/// and has sufficient capacity to accommodate the number of elements copied.
		/// </summary>
		/// <param name="source">The collection whose elements are copied to the new list</param>
		public List(IEnumerable<T> source) : base(source) { }

		/// <summary>
		/// Initializes a new instance of the class that contains elements copied from the specified collection
		/// and has sufficient capacity to accommodate the number of elements copied.
		/// </summary>
		/// <param name="source">The collection whose elements are copied to the new list.</param>
		public List(IEnumerable<ParentT> source) : base(source.Cast<T>()) { }

		/// <summary>Exposes a read only version of this list</summary>
		public IListViewer<T> ListViewer => (listViewer = listViewer ?? new ListViewer<T>(this));

		#region IList<ParentT> Members
		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"
		/// />.
		/// </summary>
		/// <param name="item">
		/// The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </param>
		int IList<ParentT>.IndexOf(ParentT item)
		{
			return IndexOf(CastToType(item, false));
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
		/// </summary>
		/// <param name="index">	  The zero-based index at which <paramref name="item" /> should be
		/// 						  inserted.</param>
		/// <param name="item">		  The object to insert into the list</param>
		void IList<ParentT>.Insert(int index, ParentT item)
		{
			Insert(index, CastToType(item, true));
		}

		/// <summary>Gets or sets items in the list</summary>
		ParentT IList<ParentT>.this[int index]
		{
			get { return this[index]; }
			set { this[index] = CastToType(value, true); }
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item"> The object to add to the list </param>
		void ICollection<ParentT>.Add(ParentT item)
		{
			Add(CastToType(item, true));
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a
		/// specific value.
		/// </summary>
		/// <param name="item">
		/// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </param>
		bool ICollection<ParentT>.Contains(ParentT item)
		{
			return Contains(CastToType(item, false));
		}

		/// <summary>copy contents of list into array</summary>
		/// <param name="array">array to fill</param>
		/// <param name="arrayIndex">starting index to fill</param>
		void ICollection<ParentT>.CopyTo(ParentT[] array, int arrayIndex)
		{
			CopyTo(array.Select(o => CastToType(o, true)).ToArray(), arrayIndex);
		}

		/// <summary> Removes the first occurrence of a specific object from the list </summary>
		/// <param name="item">
		/// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </param>
		bool ICollection<ParentT>.Remove(ParentT item)
		{
			T obj = CastToType(item, false);
			return (obj != null && Remove(obj));
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is
		/// read-only.
		/// </summary>
		bool ICollection<ParentT>.IsReadOnly => ((ICollection<T>)this).IsReadOnly;

		/// <summary> Returns an enumerator that iterates through the list</summary>
		IEnumerator<ParentT> IEnumerable<ParentT>.GetEnumerator()
		{
			foreach (var item in this)
				yield return item;
		}
		#endregion

		/// <summary>Cast to type. </summary>
		/// <param name="obj">The object.</param>
		/// <param name="useException">true to use exception.</param>
		/// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
		private static T CastToType(ParentT obj, bool useException)
		{
			if (obj == null)
				return null;
			var newObj = obj as T;
			if (useException && newObj == null)
				throw new ArgumentException("This list can only contain " + typeof(T).Name);
			return newObj;
		}
	}
}
