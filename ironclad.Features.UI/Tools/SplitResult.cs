using System.Collections.Generic;

namespace ironclad.Features.UI.Tools
{
	/// <summary> Class MatchResult. </summary>
	/// <typeparam name="T"></typeparam>
	public class MatchResult<T>
	{
		/// <summary> Initializes a new instance of the <see cref="MatchResult{T}"/> class. </summary>
		public MatchResult()
		{
			Matches = new List<T>();
			Mismatches = new List<T>();
		}

		/// <summary> Matches the specified item. </summary>
		/// <param name="item"> The item. </param>
		/// <param name="test"> if set to <c>true</c> [test]. </param>
		public void Match(T item, bool test)
		{
			if (test)
				Matches.Add(item);
			else
				Mismatches.Add(item);
		}

		/// <summary> Gets the matches. </summary>
		/// <value> The matches. </value>
		public List<T> Matches { get; }
		/// <summary> Gets the mismatches. </summary>
		/// <value> The mismatches. </value>
		public List<T> Mismatches { get; }
	}
}
