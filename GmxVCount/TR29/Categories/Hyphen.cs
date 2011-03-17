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
		/// Members of the vowels category.
		/// </summary>
		public static char[] hyphen = new char[] 
		{ 
			'\u002D', // HYPHEN-MINUS
			'\u2010', // HYPHEN
			'\u058A', // ARMENIAN HYPHEN 
			'\u30A0'  // KATAKANA-HIRAGANA DOUBLE HYPHEN
		};
	}
}