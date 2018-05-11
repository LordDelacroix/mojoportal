using System.Collections.Generic;

namespace ironclad.Features.UI.Tools
{
	/// <summary>Interface for a read only list. </summary>
	/// <remarks>Bagnes, Aug 2009.</remarks>
	public interface IListViewer<TVal> : IEnumerable<TVal>, ICountable
	{
		/// <summary>Gets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the list.</exception>
		TVal this[int index] { get; }
	}
}
