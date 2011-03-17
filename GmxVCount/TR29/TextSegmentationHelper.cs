using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GmxVCount.TR29
{
	/// <summary>
	/// A helper class to provide text segmentation assistance.
	/// </summary>
	public static class TextSegmentationHelper
	{
		/// <summary>
		/// Determines whether a given character is within a particular category.
		/// </summary>
		/// <param name="category">The category to check.</param>
		/// <param name="c">The character to check whether it's in the specified category.</param>
		/// <returns>True if the character is found within the specified category, false if not.</returns>
		public static bool IsInCategory(UnicodeWordBreakCategory category, char c)
		{
			switch (category)
			{
				case UnicodeWordBreakCategory.ALetter:
					return Categories.aletter.Contains(c);
				case UnicodeWordBreakCategory.Apostrophe:
					return Categories.apostrophe.Contains(c);
				case UnicodeWordBreakCategory.CR:
					return Categories.cr.Contains(c);
				case UnicodeWordBreakCategory.Extend:
					return Categories.extend.Contains(c);
				case UnicodeWordBreakCategory.ExtendNumLet:
					return Categories.extendnumlet.Contains(c);
				case UnicodeWordBreakCategory.Format:
					return Categories.format.Contains(c);
				case UnicodeWordBreakCategory.Katakana:
					return Categories.katakana.Contains(c);
				case UnicodeWordBreakCategory.LF:
					return Categories.lf.Contains(c);
				case UnicodeWordBreakCategory.MidLetter:
					return Categories.midletter.Contains(c);
				case UnicodeWordBreakCategory.MidNum:
					return Categories.midnum.Contains(c);
				case UnicodeWordBreakCategory.MidNumLet:
					return Categories.midnumlet.Contains(c);
				case UnicodeWordBreakCategory.Newline:
					return Categories.newline.Contains(c);
				case UnicodeWordBreakCategory.Numeric:
					return Categories.numeric.Contains(c);
				case UnicodeWordBreakCategory.Vowel:
					return Categories.vowel.Contains(c);
				case UnicodeWordBreakCategory.Hyphen:
					return Categories.hyphen.Contains(c);
				default:
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The UnicodeWordBreakCategory of {0} was not recognised.", category));
			}
		}
			}
}
