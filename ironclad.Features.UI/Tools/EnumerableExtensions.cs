using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ironclad.Features.UI.Tools
{
	/// <summary> Enumerable extensions. </summary>
	/// <remarks> Bagnes, Aug 2009. </remarks>
	public static class EnumerableExtensions
	{
		/// <summary> Ignores the nulls. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <returns> IEnumerable&lt;T&gt;. </returns>
		public static IEnumerable<T> IgnoreNulls<T>(this IEnumerable<T> source) where T : class
		{
			return source.Where(item => item != null);
		}

		/// <summary> Distincts the specified key selector. </summary>
		/// <typeparam name="TItem"> The type of the t item. </typeparam>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="keySelector"> The key selector. </param>
		/// <returns> IEnumerable&lt;TItem&gt;. </returns>
		public static IEnumerable<TItem> Distinct<TItem, TKey>(this IEnumerable<TItem> source, Func<TItem, TKey> keySelector)
		{
			var map = new Dictionary<TKey, TItem>();
			foreach (var item in source)
			{
				var key = keySelector(item);
				if (!map.ContainsKey(key))
				{
					map.Add(key, item);
					yield return item;
				}
			}
		}

		/// <summary> Orders the by. </summary>
		/// <typeparam name="TSource"> The type of the t source. </typeparam>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="descending"> if set to <c>true</c> [descending]. </param>
		/// <param name="keySelector"> The key selector. </param>
		/// <returns> IEnumerable&lt;TSource&gt;. </returns>
		public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, bool descending, Func<TSource, TKey> keySelector)
		{
			var orderedSrc = source as IOrderedEnumerable<TSource>;
			return (orderedSrc == null
				? (descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector))
				: (descending ? orderedSrc.ThenByDescending(keySelector) : orderedSrc.ThenBy(keySelector)));
		}

		/// <summary> Determines whether the specified source is empty. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <returns> <c>true</c> if the specified source is empty; otherwise, <c>false</c>. </returns>
		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.GetEnumerator().MoveNext();
		}

		/// <summary> Finds the index. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="predicate"> The predicate. </param>
		/// <returns> System.Int32. </returns>
		public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			using (var iter = source.GetEnumerator())
			{
				for (var index = 0; iter.MoveNext(); ++index)
					if (predicate(iter.Current))
						return index;
			}

			return -1;
		}

		/// <summary> iterate through each value and perform specified action </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source list for the operation. </param>
		/// <param name="action"> The action delegate. </param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			if (source != null && action != null)
				foreach (var item in source)
					action(item);
		}

		/// <summary> iterate through each value and perform specified action </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source list for the operation. </param>
		/// <param name="action"> The action delegate. </param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			var index = 0;
			if (source != null && action != null)
				foreach (var item in source)
					action(item, index++);
		}

		/// <summary> Fors the each. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach<T>(this IEnumerable source, Action<T> action)
		{
			if (source != null && action != null)
				foreach (T item in source)
					action(item);
		}

		/// <summary> Fors the every. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		/// <returns> List&lt;T&gt;. </returns>
		public static List<T> ForEvery<T>(this IEnumerable<T> source, Action<T> action)
		{
			var sourceList = source?.ToList();
			if (source != null && action != null)
				foreach (var item in sourceList)
					action(item);

			return sourceList;
		}

		/// <summary> Fors the each. </summary>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach(this StringDictionary source, Action<DictionaryEntry> action)
		{
			if (source != null && action != null)
				foreach (DictionaryEntry item in source)
					action(item);
		}

		/// <summary> Fors the each. </summary>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach(this StringDictionary source, Action<DictionaryEntry, int> action)
		{
			var index = 0;
			if (source != null && action != null)
				foreach (DictionaryEntry item in source)
					action(item, index++);
		}

		/// <summary> Fors the each. </summary>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach(this IDictionary source, Action<DictionaryEntry> action)
		{
			if (source != null && action != null)
				foreach (DictionaryEntry item in source)
					action(item);
		}

		/// <summary> Fors the each. </summary>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach(this IDictionary source, Action<DictionaryEntry, int> action)
		{
			var index = 0;
			if (source != null && action != null)
				foreach (DictionaryEntry item in source)
					action(item, index++);
		}

		/// <summary> Fors the each. </summary>
		/// <typeparam name="T0"> The type of the t0. </typeparam>
		/// <typeparam name="T1"> The type of the t1. </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach<T0, T1>(this IDictionary source, Action<T0, T1> action)
		{
			if (source != null && action != null)
				foreach (DictionaryEntry item in source)
					action((T0)(item.Key), (T1)(item.Value));
		}

		/// <summary> Fors the each. </summary>
		/// <typeparam name="T0"> The type of the t0. </typeparam>
		/// <typeparam name="T1"> The type of the t1. </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="action"> The action. </param>
		public static void ForEach<T0, T1>(this IDictionary source, Action<T0, T1, int> action)
		{
			var index = 0;
			if (source != null && action != null)
				foreach (DictionaryEntry item in source)
					action((T0)(item.Key), (T1)(item.Value), index++);
		}

		/// <summary> iterate through each value in the source list and sort the items into 2
		/// lists using the test expression passed to this method </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source list for the operation. </param>
		/// <param name="test"> The condition used to determine a match or mismatch. </param>
		/// <returns> MatchResult&lt;T&gt;. </returns>
		public static MatchResult<T> Match<T>(this IEnumerable<T> source, Func<T, bool> test)
		{
			var result = new MatchResult<T>();
			if (source != null && test != null)
				foreach (var item in source)
					result.Match(item, test(item));
			return result;
		}

		/// <summary> Append an item to the end of an enumerable list of items. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source for the append operation. </param>
		/// <param name="item"> The item to append. </param>
		/// <returns> IEnumerable&lt;T&gt;. </returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item) where T : class
		{
			foreach (var t in source)
				yield return t;
			if (item != null)
				yield return item;
		}

		/// <summary> Append multiple items to the end of an enumerable list of items. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source for the append operation. </param>
		/// <param name="items"> The items to append. </param>
		/// <returns> IEnumerable&lt;T&gt;. </returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, IEnumerable<T> items) where T : class
		{
			foreach (var t in source)
				yield return t;
			if (items != null)
				foreach (var t in items)
					yield return t;
		}

		/// <summary> Append multiple items to the end of an enumerable list of items. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source for the append operation. </param>
		/// <param name="itemsList"> The items to append. </param>
		/// <returns> IEnumerable&lt;T&gt;. </returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, params IEnumerable<T>[] itemsList) where T : class
		{
			foreach (var t in source)
				yield return t;
			if (itemsList != null && itemsList.Length > 0)
				foreach (var items in itemsList)
					foreach (var t in items)
						yield return t;
		}

		/// <summary> Prepend an item to the beginning of an enumerable list of items. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source for the prepend operation. </param>
		/// <param name="items"> The items to prepend. </param>
		/// <returns> IEnumerable&lt;T&gt;. </returns>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, params T[] items) where T : class
		{
			foreach (var item in items)
				yield return item;
			foreach (var t in source)
				yield return t;
		}

		/// <summary> Prepend an item to the beginning of an enumerable list of items. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> Source for the prepend operation. </param>
		/// <param name="item"> The item to prepend. </param>
		/// <returns> IEnumerable&lt;T&gt;. </returns>
		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item) where T : class
		{
			if (item != null)
				yield return item;
			foreach (var t in source)
				yield return t;
		}

		/// <summary> merge two enumerables into a single list of pairs of items (matching items by index) </summary>
		/// <typeparam name="T1"> The type of the t1. </typeparam>
		/// <typeparam name="T2"> The type of the t2. </typeparam>
		/// <param name="source1"> Source 1. </param>
		/// <param name="source2"> Source 2. </param>
		/// <returns> IEnumerable&lt;Pair&lt;T1, T2&gt;&gt;. </returns>
		public static IEnumerable<Pair<T1, T2>> Merge<T1, T2>(this IEnumerable<T1> source1,
			IEnumerable<T2> source2)
		{
			var e1 = source1.GetEnumerator();
			var e2 = source2.GetEnumerator();
			while (e1.MoveNext() && e2.MoveNext())
				yield return new Pair<T1, T2>(e1.Current, e2.Current);
		}

		/// <summary> Transform a list of x  into a list of y (where x may produce zero, one, or many y) </summary>
		/// <typeparam name="TSrc"> The type of the t source. </typeparam>
		/// <typeparam name="TDest"> The type of the t dest. </typeparam>
		/// <param name="source"> Source for the transform. </param>
		/// <param name="func"> The transform func. </param>
		/// <returns> IEnumerable&lt;TDest&gt;. </returns>
		public static IEnumerable<TDest> Transform<TSrc, TDest>(this IEnumerable<TSrc> source,
			Func<TSrc, IEnumerable<TDest>> func)
			where TSrc : class
			where TDest : class
		{
			foreach (var src in source)
				if (src == null)
					yield return null;
				else
				{
					var itemEnum = func(src);
					if (itemEnum != null)
						foreach (var item in itemEnum)
							yield return item;
				}
		}

		/// <summary> Transform a linked list of x  into an enumerable of x </summary>
		/// <typeparam name="TSrc"> The type of the t source. </typeparam>
		/// <param name="source"> Source for the transform. </param>
		/// <param name="nextFunc"> The transform func. </param>
		/// <returns> IEnumerable&lt;TSrc&gt;. </returns>
		public static IEnumerable<TSrc> IterateList<TSrc>(this TSrc source, Func<TSrc, TSrc> nextFunc)
			where TSrc : class
		{
			for (var item = source; item != null; item = nextFunc(item))
				yield return item;
		}

		/// <summary> Transform a linked list of x  into an enumerable of x </summary>
		/// <typeparam name="TSrc"> The type of the t source. </typeparam>
		/// <typeparam name="TDest"> The type of the t dest. </typeparam>
		/// <param name="source"> Source for the transform. </param>
		/// <param name="headFunc"> get head of dest from source </param>
		/// <param name="nextFunc"> The transform func. </param>
		/// <returns> IEnumerable&lt;Pair&lt;TSrc, TDest&gt;&gt;. </returns>
		public static IEnumerable<Pair<TSrc, TDest>> TransformList<TSrc, TDest>(this IEnumerable<TSrc> source, Func<TSrc, TDest> headFunc, Func<TDest, TDest> nextFunc)
			where TSrc : class
			where TDest : class
		{
			if (source != null)
				foreach (var src in source)
					for (var item = headFunc(src); item != null; item = nextFunc(item))
						yield return new Pair<TSrc, TDest>(src, item);
		}

		/// <summary> Transform a list of x  into a list of y (where x may produce zero, one, or many y) </summary>
		/// <typeparam name="TSrc"> The type of the t source. </typeparam>
		/// <typeparam name="TDest"> The type of the t dest. </typeparam>
		/// <param name="source"> Source for the transform. </param>
		/// <param name="func"> The transform func. </param>
		/// <returns> IEnumerable&lt;Pair&lt;TSrc, TDest&gt;&gt;. </returns>
		public static IEnumerable<Pair<TSrc, TDest>> TransformPairs<TSrc, TDest>(this IEnumerable<TSrc> source,
			Func<TSrc, IEnumerable<TDest>> func)
		{
			foreach (var src in source)
			{
				var itemEnum = func(src);
				if (itemEnum == null)
					yield return null;
				else
					foreach (var item in itemEnum)
						yield return new Pair<TSrc, TDest>(src, item);
			}
		}

		/// <summary> Alls the specified function. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="func"> The function. </param>
		/// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
		public static bool? All<T>(this IEnumerable<T> source, Func<T, bool?> func)
		{
			bool? result = true;
			foreach (var item in source)
			{
				result = func(item);
				if (!result.HasValue || !result.Value)
					break;
			}
			return result;
		}

		/// <summary> Anies the specified function. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="source"> The source. </param>
		/// <param name="func"> The function. </param>
		/// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
		public static bool? Any<T>(this IEnumerable<T> source, Func<T, bool?> func)
		{
			bool? result = false;
			foreach (var item in source)
			{
				result = func(item);
				if (!result.HasValue || result.Value)
					break;
			}
			return result;
		}

		/// <summary> Ins the list. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="item"> The item. </param>
		/// <param name="list"> The list. </param>
		/// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
		public static bool InList<T>(this T item, params T[] list)
		{
			return list.Contains(item);
		}

		/// <summary> Converts/Appends an enumerable of item into a list of item </summary>
		/// <typeparam name="TItem"> The type of the t item. </typeparam>
		/// <param name="source"> the source enumerable. </param>
		/// <param name="list"> The list to append item into. </param>
		/// <returns> IList&lt;TItem&gt;. </returns>
		public static IList<TItem> ToList<TItem>(this IEnumerable<TItem> source, IList<TItem> list)
		{
			if (list == null)
				return source.ToList();
			source.ForEach(list.Add);
			return list;
		}

		/// <summary> Converts an enumerable of item into a list of item that can be accessed as IList of
		/// parent item </summary>
		/// <typeparam name="T">  </typeparam>
		/// <typeparam name="TParent"> The type of the t parent. </typeparam>
		/// <param name="source"> the source enumerable. </param>
		/// <returns> List&lt;T, TParent&gt;. </returns>
		public static List<T, TParent> ToList<T, TParent>(this IEnumerable<T> source)
			where T : class, TParent
			where TParent : class
		{
			return new List<T, TParent>(source);
		}

		/// <summary> Converts an enumerable of item into a list of item that can be accessed as IList of
		/// parent item </summary>
		/// <typeparam name="T">  </typeparam>
		/// <typeparam name="TParent"> The type of the t parent. </typeparam>
		/// <param name="source"> the source enumerable. </param>
		/// <returns> List&lt;T, TParent&gt;. </returns>
		public static List<T, TParent> ToList<T, TParent>(this IEnumerable<TParent> source)
			where T : class, TParent
			where TParent : class
		{
			return new List<T, TParent>(source);
		}

		/// <summary> Inserts the ordered. </summary>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <param name="list"> The list. </param>
		/// <param name="value"> The value. </param>
		/// <param name="isAscending"> if set to <c>true</c> [is ascending]. </param>
		/// <param name="allowDup"> if set to <c>true</c> [allow dup]. </param>
		/// <param name="getKey"> The get key. </param>
		/// <returns> TVal. </returns>
		public static TVal InsertOrdered<TVal, TKey>(this List<TVal> list, TVal value, bool isAscending, bool allowDup,
			Func<TVal, TKey> getKey) where TKey : IComparable
		{
			var key = getKey(value);
			var pos = isAscending
				? list.FindLastIndex(item => key.CompareTo(getKey(item)) <= 0)
				: list.FindLastIndex(item => key.CompareTo(getKey(item)) >= 0);
			if (!allowDup && pos >= 0 && key.CompareTo(getKey(list[pos])) == 0)
				return list[pos];
			list.Insert(pos + 1, value);
			return value;
		}

		/// <summary> Differences the specified orig list. </summary>
		/// <typeparam name="TVal"> The type of the val. </typeparam>
		/// <typeparam name="TKey"> The type of the key. </typeparam>
		/// <param name="origList"> The orig list. </param>
		/// <param name="newlist"> The newlist. </param>
		/// <param name="getKey"> The get key. </param>
		/// <returns> IEnumerable&lt;TVal&gt;. </returns>
		public static IEnumerable<TVal> Difference<TVal, TKey>(this IEnumerable<TVal> origList, IEnumerable<TVal> newlist,
			Func<TVal, TKey> getKey)
		{
			var newMap = newlist.ToMap(getKey);
			return origList.Where(i => !newMap.ContainsKey(getKey(i)));
		}

		/// <summary> get the differences between two lists </summary>
		/// <typeparam name="TVal1"> The type of the val1. </typeparam>
		/// <typeparam name="TVal2"> The type of the val2. </typeparam>
		/// <typeparam name="TKey"> The type of the key. </typeparam>
		/// <param name="origList"> The orig list. </param>
		/// <param name="newlist"> The newlist. </param>
		/// <param name="getKey1"> The get key1. </param>
		/// <param name="getKey2"> The get key2. </param>
		/// <returns> IEnumerable&lt;TVal1&gt;. </returns>
		public static IEnumerable<TVal1> Difference<TVal1, TVal2, TKey>(this IEnumerable<TVal1> origList, IEnumerable<TVal2> newlist,
			Func<TVal1, TKey> getKey1, Func<TVal2, TKey> getKey2)
		{
			var newMap = newlist.ToMap(getKey2);
			return origList.Where(i => !newMap.ContainsKey(getKey1(i)));
		}

		/// <summary> returns a list of incremental integers </summary>
		/// <param name="start"> The starting number. </param>
		/// <param name="count"> Number of integers to return. </param>
		/// <returns> IEnumerable&lt;System.Int32&gt;. </returns>
		public static IEnumerable<int> Next(this int start, int count)
		{
			while ((count--) > 0)
				yield return (start++);
		}

		/// <summary> returns a list of incremental integers. </summary>
		/// <param name="start"> The starting number. </param>
		/// <param name="count"> Number of integers to return. </param>
		/// <param name="step"> Amount to increment by. </param>
		/// <returns> IEnumerable&lt;System.Int32&gt;. </returns>
		public static IEnumerable<int> Next(this int start, int count, int step)
		{
			while ((count--) > 0)
			{
				yield return start;
				start += step;
			}
		}

		/// <summary> returns a list of incremental dates. </summary>
		/// <param name="start"> The starting date. </param>
		/// <param name="count"> Number of integers to return. </param>
		/// <param name="step"> Amount of time to increment by. </param>
		/// <returns> IEnumerable&lt;DateTime&gt;. </returns>
		public static IEnumerable<DateTime> Next(this DateTime start, int count, TimeSpan step)
		{
			while ((count--) > 0)
			{
				yield return start;
				start += step;
			}
		}

		/// <summary> Convert list of items into a string representation. </summary>
		/// <param name="list"> The list. </param>
		/// <param name="sep"> The seperator string </param>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public static string ToString(this IEnumerable list, string sep)
		{
			if (list == null)
				return "";
			var buf = new StringBuilder();
			foreach (var item in list)
				buf.Append(buf.Length > 0
					? sep
					: string.Empty).Append(item);
			return buf.ToString();
		}

		/// <summary> Convert list of items into a string representation. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="list"> The list. </param>
		/// <param name="sep"> The seperator string </param>
		/// <param name="printFunc"> what to print for eac item </param>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public static string ToString<T>(this IEnumerable<T> list, string sep, Func<T, object> printFunc)
		{
			//return (list == null)
			//    ? null
			//    : list.Aggregate(new StringBuilder(), (buf, item) => buf.Append(buf.Length == 0 ? "" : sep).Append(item)).ToString();
			if (list == null)
				return "";
			var buf = new StringBuilder();
			foreach (var item in list)
			{
				var val = printFunc(item);
				if (val != null)
					buf.Append(buf.Length > 0
						? sep
						: string.Empty).Append(val);
			}
			return buf.ToString();
		}

		/// <summary> Convert list of items into a string representation. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="list"> The list. </param>
		/// <param name="sep"> The seperator string </param>
		/// <param name="printFunc"> what to print for eac item </param>
		/// <returns> A <see cref="System.String" /> that represents this instance. </returns>
		public static string ToString<T>(this IEnumerable<T> list, string sep, Func<T, int, object> printFunc)
		{
			if (list == null)
				return "";
			var buf = new StringBuilder();
			var pos = 0;
			foreach (var item in list)
			{
				var val = printFunc(item, pos++);
				if (val != null)
					buf.Append(buf.Length > 0
						? sep
						: string.Empty).Append(val);
			}
			return buf.ToString();
		}

		/// <summary> converts an list of items into a dictionary. Key difference between ToMap() and
		/// ToDictionary is that ToDictionary will through execption if duplicate keys are added, ToMap
		/// will replace items (so last item in list with identical key remains in dictionary </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="keyFunc"> function to extract key value from item </param>
		/// <returns> Dictionary&lt;TKey, TVal&gt;. </returns>
		public static Dictionary<TKey, TVal> ToMap<TKey, TVal>(this IEnumerable<TVal> list,
			Func<TVal, TKey> keyFunc)
		{
			return ToMap(list, (Dictionary<TKey, TVal>)null, keyFunc);
		}


		/// <summary> To the map. </summary>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list. </param>
		/// <param name="sepEl"> The sep el. </param>
		/// <param name="sepKV"> The sep kv. </param>
		/// <param name="valFunc"> The value function. </param>
		/// <returns> Dictionary&lt;System.String, TVal&gt;. </returns>
		/// <exception cref="System.ArgumentException"> bad kv pairs: " + list </exception>
		public static Dictionary<string, TVal> ToMap<TVal>(this string list, char sepEl, char sepKV,
			Func<string, TVal> valFunc)
		{
			var map = new Dictionary<string, TVal>();
			if (!list.IsNullOrEmpty() && list.Contains(sepKV))
				foreach (var kvPair in list.Split(sepEl).Select(kv => kv.Split(sepKV)))
				{
					if (kvPair.Length != 2)
						throw new ArgumentException("bad kv pairs: " + list);
					map[kvPair[0]] = valFunc(kvPair[1]);
				}
			return map;
		}

		/// <summary> converts an list of items into a dictionary. Key difference between ToMap() and
		/// ToDictionary is that ToDictionary will through execption if duplicate keys are added, ToMap
		/// will replace items (so last item in list with identical key remains in dictionary </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="comp"> comparer to use in map </param>
		/// <param name="keyFunc"> function to extract key value from item </param>
		/// <returns> Dictionary&lt;TKey, TVal&gt;. </returns>
		public static Dictionary<TKey, TVal> ToMap<TKey, TVal>(this IEnumerable<TVal> list,
			IEqualityComparer<TKey> comp, Func<TVal, TKey> keyFunc)
		{
			return ToMap(list, new Dictionary<TKey, TVal>(comp), keyFunc);
		}

		/// <summary> converts an list of items into a dictionary. Key difference between ToMap() and
		/// ToDictionary is that ToDictionary will through execption if duplicate keys are added, ToMap
		/// will replace items (so last item in list with identical key remains in dictionary </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="map"> dictionary to load with values </param>
		/// <param name="keyFunc"> function to extract key value from item </param>
		/// <returns> Dictionary&lt;TKey, TVal&gt;. </returns>
		public static Dictionary<TKey, TVal> ToMap<TKey, TVal>(this IEnumerable<TVal> list,
			Dictionary<TKey, TVal> map, Func<TVal, TKey> keyFunc)
		{
			map = map ?? new Dictionary<TKey, TVal>();
			if (list != null)
				list.ForEach(i => map[keyFunc(i)] = i);
			return map;
		}

		/// <summary> converts an list of items into a dictionary. Key difference between ToMap() and
		/// ToDictionary is that ToDictionary will through execption if duplicate keys are added, ToMap
		/// will replace items (so last item in list with identical key remains in dictionary </summary>
		/// <typeparam name="TSrc"> The type of the t source. </typeparam>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="map"> dictionary to load with values </param>
		/// <param name="keyFunc"> function to extract key from item </param>
		/// <param name="valueFunc"> function to extract value from item </param>
		/// <returns> Dictionary&lt;TKey, TVal&gt;. </returns>
		public static Dictionary<TKey, TVal> ToMap<TSrc, TKey, TVal>(this IEnumerable<TSrc> list,
			Dictionary<TKey, TVal> map, Func<TSrc, TKey> keyFunc, Func<TSrc, TVal> valueFunc)
		{
			map = map ?? new Dictionary<TKey, TVal>();
			if (list != null)
				list.ForEach(i => map[keyFunc(i)] = valueFunc(i));
			return map;
		}

		/// <summary> do a pathwise best match of a string (simular to loggers in log4net) </summary>
		/// <typeparam name="V">  </typeparam>
		/// <param name="map"> The map to search. </param>
		/// <param name="key"> The key. </param>
		/// <param name="sep"> The seperator. </param>
		/// <returns> System.Nullable&lt;V&gt;. </returns>
		public static V? MatchPath<V>(this Dictionary<string, V> map, string key, string sep) where V : struct
		{
			for (V val; !key.IsNullOrEmpty(); key = key.RemoveLastWord(sep))
				if (map.TryGetValue(key, out val))
					return val;
			return null;
		}

		/// <summary> Converts a list of items into a read only list. </summary>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list. </param>
		/// <returns> ListViewer&lt;TVal&gt;. </returns>
		public static ListViewer<TVal> ToReadOnlyList<TVal>(this IEnumerable<TVal> list)
		{
			return new ListViewer<TVal>(list);
		}

		/// <summary> Converts a list of items into a read only map. </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list. </param>
		/// <param name="comp"> The comparer. </param>
		/// <param name="defVal"> The default value. </param>
		/// <param name="keyFunc"> The key func. </param>
		/// <returns> MapViewer&lt;TKey, TVal&gt;. </returns>
		public static MapViewer<TKey, TVal> ToReadOnlyMap<TKey, TVal>(this IEnumerable<TVal> list,
			IEqualityComparer<TKey> comp, TVal defVal, Func<TVal, TKey> keyFunc)
		{
			return new MapViewer<TKey, TVal>(ToMap(list, comp, keyFunc), defVal);
		}

		/// <summary> converts an list of items into a read only dictionary. </summary>
		/// <typeparam name="TSrc"> The type of the t source. </typeparam>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="keyFunc"> function to extract key from item </param>
		/// <param name="valueFunc"> function to extract value from item </param>
		/// <returns> MapViewer&lt;TKey, TVal&gt;. </returns>
		public static MapViewer<TKey, TVal> ToReadOnlyMap<TSrc, TKey, TVal>(this IEnumerable<TSrc> list,
			Func<TSrc, TKey> keyFunc, Func<TSrc, TVal> valueFunc) where TVal : class
		{
			return new MapViewer<TKey, TVal>(list.ToMap(null, keyFunc, valueFunc), null);
		}

		/// <summary> converts an list of items into a read only dictionary. </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="keyFunc"> function to extract key from item </param>
		/// <returns> MapViewer&lt;TKey, TVal&gt;. </returns>
		public static MapViewer<TKey, TVal> ToReadOnlyMap<TKey, TVal>(this IEnumerable<TVal> list,
			Func<TVal, TKey> keyFunc) where TVal : class
		{
			return new MapViewer<TKey, TVal>(list.ToMap(keyFunc), null);
		}

		/// <summary> converts an list of items into a read only dictionary. </summary>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="list"> The list to insert. </param>
		/// <param name="keyFunc"> function to extract key from item </param>
		/// <returns> MapViewer&lt;System.String, TVal&gt;. </returns>
		public static MapViewer<string, TVal> ToStringReadOnlyMap<TVal>(this IEnumerable<TVal> list,
			Func<TVal, string> keyFunc) where TVal : class
		{
			return new MapViewer<string, TVal>(list.ToMap(StringComparer.InvariantCultureIgnoreCase, keyFunc), null);
		}

		/// <summary> used because I hate creating temp variables to pass in as out parameters </summary>
		/// <typeparam name="TKey"> The type of the t key. </typeparam>
		/// <typeparam name="TVal"> The type of the t value. </typeparam>
		/// <param name="map"> The dictionary to search. </param>
		/// <param name="key"> The key to find. </param>
		/// <returns> TVal. </returns>
		public static TVal TryGetValue<TKey, TVal>(this IDictionary<TKey, TVal> map, TKey key)
			where TVal : class
		{
			TVal val;
			return (map.TryGetValue(key, out val)
				? val
				: null);
		}

		/// <summary> break an enumerable into multiple smaller chunks </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="list"> The list. </param>
		/// <param name="size"> The max size of a chunk. </param>
		/// <returns> IEnumerable&lt;IEnumerable&lt;T&gt;&gt;. </returns>
		public static IEnumerable<IEnumerable<T>> SegmentList<T>(this IEnumerable<T> list, int size)
		{
			var buf = new T[size];
			var pos = 0;
			foreach (var item in list)
			{
				buf[pos] = item;
				if (++pos >= size)
				{
					yield return buf;
					pos = 0;
				}
			}
			if (pos > 0)
				yield return buf.Take(pos);
		}

		/// <summary> Updates a list to contain the same (or equivilent) items as the enumerable.  This is
		/// used when copying items into observable collections so that visual state is not lost when updating
		/// a data source in WPF. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="src"> items in enumerable to load. </param>
		/// <param name="dest"> the collection to hold the duplicate image </param>
		/// <param name="keyString"> The key string. </param>
		/// <param name="copyFunc"> The copy func. </param>
		public static void UpdateList<T>(this IEnumerable<T> src, ICollection<T> dest,
			Func<T, string> keyString, Action<T, T> copyFunc)
		{
			// if desc is empty just copy all items
			if (dest.Count == 0)
			{
				src.ForEach(dest.Add);
				return;
			}

			// if source is empty remove all items
			if (src == null || !src.Any())
			{
				dest.Clear();
				return;
			}

			// create sorted enumerators for src and dest
			var sortedSrc = src.Select(i => new
				{
					Key = keyString(i) ?? string.Empty,
					Value = i
				}).OrderBy(p => p.Key).GetEnumerator();
			var sortedDest = dest.Select(i => new
				{
					Key = keyString(i) ?? string.Empty,
					Value = i
				}).OrderBy(p => p.Key).GetEnumerator();

			// create working variables for loop
			var comp = 0;
			var srcKey = string.Empty;
			var destKey = string.Empty;

			while (true)
			{
				// update key values for src and dest (if at end of enumerator use null to flag end)
				if (srcKey != null && comp <= 0)
					srcKey = (sortedSrc.MoveNext()
						? sortedSrc.Current.Key
						: null);
				if (destKey != null && comp >= 0)
					destKey = (sortedDest.MoveNext()
						? sortedDest.Current.Key
						: null);

				// loop while there are still values in src or dest
				if (srcKey == null && destKey == null)
					break;

				// compare keys
				comp = (srcKey == null
					? 1
					: (destKey == null
						? -1
						: string.Compare(srcKey, destKey)));
				// because enumerators are sorted the following assumptions are true:
				//    src<dest...  missing key (add item),
				//    src>dest...  removed key (delete item),
				//    src==dest... matching key (item may have changed so copy)
				if (comp < 0)
					dest.Add(sortedSrc.Current.Value);
				else if (comp > 0)
					dest.Remove(sortedDest.Current.Value);
				else
					copyFunc?.Invoke(sortedDest.Current.Value, sortedSrc.Current.Value);
			}
		}

		/// <summary> sort list of items. </summary>
		/// <typeparam name="T">  </typeparam>
		/// <param name="src"> list to sort </param>
		/// <param name="keyString"> The key string. </param>
		/// <param name="moveFunc"> The move func. </param>
		public static void Sort<T>(this IEnumerable<T> src, Func<T, string> keyString,
			Action<int, int> moveFunc)
		{
			var newIdx = -1;
			var items = src.Select((item, idx) => new IndexedItem<T>
			{
				Item = item,
				Index = idx
			}).OrderBy(i => keyString(i.Item)).ToList();

			foreach (var item in items)
				if (++newIdx != item.Index)
				{
					moveFunc(item.Index, newIdx);
					items.Where(i => i.Index >= newIdx && i.Index < item.Index).ForEach(i => ++i.Index);
				}
		}

		/// <summary> Indexed item used by sort method </summary>
		/// <typeparam name="T"></typeparam>
		/// <remarks> Bagnes, Aug 2009. </remarks>
		private class IndexedItem<T>
		{
			/// <summary> Gets or sets the item. </summary>
			/// <value> The item. </value>
			public T Item { get; set; }
			/// <summary> Gets or sets the index. </summary>
			/// <value> The index. </value>
			public int Index { get; set; }
		}

		/// <summary> Makes the array. </summary>
		/// <typeparam name="TElement"> The type of the t element. </typeparam>
		/// <param name="source"> The source. </param>
		/// <returns> TElement[]. </returns>
		public static TElement[] MakeArray<TElement>(this IEnumerable<TElement> source)
		{
			TElement[] items = null;
			if (source != null)
			{
				var collection = source as ICollection<TElement>;
				if (collection == null)
					items = MakeArray(0, source.GetEnumerator());
				else if (collection.Count > 0)
					collection.CopyTo(items = new TElement[collection.Count], 0);
			}
			return items ?? new TElement[0];
		}

		/// <summary> Makes the array. </summary>
		/// <typeparam name="TElement"> The type of the t element. </typeparam>
		/// <param name="startIndex"> The start index. </param>
		/// <param name="source"> The source. </param>
		/// <returns> TElement[]. </returns>
		private static TElement[] MakeArray<TElement>(int startIndex, IEnumerator<TElement> source)
		{
			var currentSize = 0;
			var segmentSize = (startIndex == 0 ? 4 : startIndex * 3);
			TElement[] result = null;
			var buffer = new TElement[segmentSize];
			while (result == null && currentSize < segmentSize)
			{
				if (source.MoveNext())
					buffer[currentSize++] = source.Current;
				else
					result = new TElement[startIndex + currentSize];
			}
			result = result ?? MakeArray(segmentSize + startIndex, source);
			if (currentSize > 0)
				Array.Copy(buffer, 0, result, startIndex, currentSize);
			return result;
		}

	}
}
