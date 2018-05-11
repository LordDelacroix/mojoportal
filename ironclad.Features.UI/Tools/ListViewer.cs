using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ironclad.Features.UI.Tools
{
	/// <summary> List viewer is a read only list </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="IListViewer{TVal}" />
	/// <remarks> Bagnes, Aug 2009. </remarks>
	[Serializable]
	public class ListViewer<T> : IListViewer<T>
	{
		/// <summary> The empty </summary>
		protected static ListViewer<T> _empty;

		/// <summary> The items </summary>
		protected IList<T> _items;

		/// <summary> Gets or sets the items. </summary>
		protected IList<T> Items
		{
			get
			{
				ReloadIfNeeded();
				return _items ?? EmptyArray<T>.Instance;
			}
			set { _items = value; }
		}

		/// <summary> Reloads if needed. </summary>
		protected virtual void ReloadIfNeeded() { }

		/// <summary> Constructor. </summary>
		/// <param name="source"> Source for the list. </param>
		public ListViewer(IEnumerable<T> source)
		{
			_items = (source as IList<T>) ?? source?.ToArray();
		}

		/// <summary> Returns an enumerator that iterates through the collection. </summary>
		/// <returns> An enumerator that can be used to iterate through the collection. </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		/// <summary> Returns an enumerator that iterates through the collection. </summary>
		/// <returns> An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection. </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		/// <summary> Gets the number of elements contained in the Collection. </summary>
		public int Count => Items.Count;

		/// <summary> Gets the empty. </summary>
		[SuppressMessage("Microsoft.Design", "CA1000", Justification = "FX Cop is wrong")]
		public static ListViewer<T> Empty => _empty ?? new ListViewer<T>(EmptyArray<T>.Instance);

		/// <summary> Gets the element at the specified index. </summary>
		/// <param name="index"> The index. </param>
		/// <returns> T. </returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the list.</exception>
		public T this[int index] => Items[index];
	}
}
