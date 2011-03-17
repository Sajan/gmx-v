using System;

namespace GmxVCount.TR29
{
	/// <summary>
	/// A static class which contains arrays of characters within 
	/// the TR29 categories.
	/// </summary>
	public static partial class Categories
	{
		/// <summary>
		/// Members of the newline category.
		/// </summary>
		public static char[] newline = new char[] {
'\u000b', 
'\u000c', 
'\u0085', 
'\u2028', 
'\u2029'
 };
	}
}