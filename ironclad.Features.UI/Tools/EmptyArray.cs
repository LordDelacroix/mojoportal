using System.Diagnostics.CodeAnalysis;

namespace ironclad.Features.UI.Tools
{
	/// <summary> Used to create a static (reusable) instance of a empty array </summary>
	/// <typeparam name="T"></typeparam>
	/// <remarks> Bagnes, Aug 2009. </remarks>
	public static class EmptyArray<T>
	{
		/// <summary> The instance </summary>
		private static T[] _instance;

		/// <summary> Gets the instance. </summary>
		/// <value> The instance. </value>
		[SuppressMessage("Microsoft.Design", "CA1000", Justification = "FX Cop is wrong")]
		public static T[] Instance => _instance ?? (_instance = new T[0]);
	}
}
