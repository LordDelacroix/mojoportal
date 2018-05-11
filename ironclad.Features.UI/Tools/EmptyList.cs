using System.Collections.Generic;

namespace ironclad.Features.UI.Tools
{
	/// <summary> Class EmptyList. </summary>
	/// <typeparam name="T"></typeparam>
	public static class EmptyList<T> where T : class
	{
		/// <summary> The instance </summary>
		private static List<T> instance;

		/// <summary> Gets the instance. </summary>
		/// <value> The instance. </value>
		public static List<T> Instance { get { return instance ?? (instance = new List<T>()); } }
	}
}
