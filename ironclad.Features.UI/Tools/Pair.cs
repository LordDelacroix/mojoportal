namespace ironclad.Features.UI.Tools
{
	/// <summary> Pair of values </summary>
	/// <typeparam name="TFirst">The type of the t first.</typeparam>
	/// <typeparam name="TSecond">The type of the t second.</typeparam>
	/// <remarks> Bagnes, Aug 2009. </remarks>
	public class Pair<TFirst, TSecond>
	{
		/// <summary> Initializes a new instance of the <see cref="Pair{TFirst, TSecond}" /> class. </summary>
		/// <param name="first"> The first. </param>
		/// <param name="second"> The second. </param>
		public Pair(TFirst first, TSecond second)
		{
			First = first;
			Second = second;
		}

		/// <summary> the first value in pair. </summary>
		/// <value> The first. </value>
		public TFirst First { get; }

		/// <summary> the second value in pair. </summary>
		/// <value> The second. </value>
		public TSecond Second { get; }
	}
}