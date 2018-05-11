using System;
using System.Collections.Generic;

namespace ironclad.Features.UI.Tools
{
	/// <summary>read only dictionary type</summary>
	/// <remarks>Bagnes, Aug 2009.</remarks>
	[Serializable]
	public class MapViewer<TKey, TVal> : IMapViewer<TKey, TVal>
	{
		protected Dictionary<TKey, TVal> itemMap;
		private readonly TVal _defaultValue;

		/// <summary>			 Constructor. </summary>
		/// <param name="source">Source for the.</param>
		/// <param name="getKey">The get key.</param>
		/// <param name="defaultValue">value to return if key is not found</param>
		public MapViewer(IEnumerable<TVal> source, TVal defaultValue, Func<TVal, TKey> getKey)
		{
			_defaultValue = defaultValue;
			GetKey = getKey;
			LoadItems(source);
		}

		/// <summary>			 Constructor. </summary>
		/// <param name="source">Source for the.</param>
		/// <param name="defaultValue">value to return if key is not found</param>
		public MapViewer(Dictionary<TKey, TVal> source, TVal defaultValue)
		{
			_defaultValue = defaultValue;
			itemMap = source;
		}

		protected TVal LoadItem(TVal item)
		{
			if (GetKey == null)
				throw new NotSupportedException();
			if (item != null)
			{
				if (itemMap == null)
					itemMap = new Dictionary<TKey, TVal>();
				itemMap[GetKey(item)] = item;
			}
			return item;
		}

		/// <summary>Loads the items. </summary>
		/// <param name="source">Source for the list.</param>
		protected void LoadItems(IEnumerable<TVal> source)
		{
			if (GetKey == null)
				throw new NotSupportedException();
			itemMap = source.ToMap(itemMap, GetKey);
		}

		public Func<TVal, TKey> GetKey { get; private set; }

		/// <summary>Search for the element with the matching key</summary>
		/// <param name="key">key to find</param>
		public virtual TVal this[TKey key]
		{
			get
			{
				TVal result;
				return (itemMap != null && itemMap.TryGetValue(key, out result) ? result : _defaultValue);
			}
		}

		/// <summary>list of all key values.</summary>
		public IEnumerable<TKey> Keys => itemMap?.Keys ?? (IEnumerable<TKey>)EmptyArray<TKey>.Instance;

		/// <summary>list of all values.</summary>
		public IEnumerable<TVal> Values => itemMap?.Values ?? (IEnumerable<TVal>)EmptyArray<TVal>.Instance;

		/// <summary>Gets the number of elements contained in the Collection.</summary>
		public int Count => itemMap?.Count ?? 0;
	}
}
