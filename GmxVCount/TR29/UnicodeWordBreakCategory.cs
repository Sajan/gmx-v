using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmxVCount.TR29
{
	/// <summary>
	/// The category of the Unicode Word Break, as defined in TR29.
	/// </summary>
	public enum UnicodeWordBreakCategory
	{
		/// <summary>
		/// The ALetter category.
		/// </summary>
		ALetter,
		/// <summary>
		/// The Apostrophe category - an optional rule for the apostrophe character 
		/// (see Unicode TR 29 Section 4)
		/// </summary>
		Apostrophe,
		/// <summary>
		/// The Carriage Return category.
		/// </summary>
		CR,
		/// <summary>
		/// The Extend category.
		/// </summary>
		Extend,
		/// <summary>
		/// The ExtendNumLet category.
		/// </summary>
		ExtendNumLet,
		/// <summary>
		/// The Format category.
		/// </summary>
		Format,
		/// <summary>
		/// The Japanese Katakana category.
		/// </summary>
		Katakana,
		/// <summary>
		/// The Line Feed category.
		/// </summary>
		LF,
		/// <summary>
		/// The MidLetter category.
		/// </summary>
		MidLetter,
		/// <summary>
		/// The MidNum category.
		/// </summary>
		MidNum,
		/// <summary>
		/// The MidNumLet category.
		/// </summary>
		MidNumLet,
		/// <summary>
		/// The New Line category.
		/// </summary>
		Newline,
		/// <summary>
		/// The Numeric category.
		/// </summary>
		Numeric,
		/// <summary>
		/// The custom vowels category, defined as part of the 
		/// Break between apostrophe and vowels (French, Italian) rule.
		/// </summary>
		Vowel,
		/// <summary>
		/// The custom hyphen category, added by GMX/V section 2.8.
		/// </summary>
		Hyphen
	}
}
