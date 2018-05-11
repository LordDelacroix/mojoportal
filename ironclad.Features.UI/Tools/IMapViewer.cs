using System.Collections.Generic;

namespace ironclad.Features.UI.Tools
{
	/// <summary>Interface for a read only dictionary type</summary>
	/// <remarks>Bagnes, Aug 2009.</remarks>
	public interface IMapViewer<TKey, TValue> : ICountable
	{
		/// <summary>Indexer to  items within this collection using array index syntax.</summary>
		TValue this[TKey key] { get; }

		/// <summary> list of all key values.</summary>
		IEnumerable<TKey> Keys { get; }

		/// <summary> list of all values.</summary>
		IEnumerable<TValue> Values { get; }
	}
}
