using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmxVCount.TextBlocks
{
	/// <summary>
	/// Represents the word boundary rules provided by TR29.
	/// </summary>
	public enum WordBoundaryRule
	{
		/// <summary>
		/// No word boundary rule is applicable.
		/// </summary>
		None,
		/// <summary>
		/// Break at the start and end of text.
		/// </summary>
		WB1,
		/// <summary>
		/// Break at the start and end of text.
		/// </summary>
		WB2,
		/// <summary>
		/// Do not break within CRLF.
		/// </summary>
		WB3,
		/// <summary>
		/// Otherwise, break before and after newlines.
		/// </summary>
		WB3a,
		/// <summary>
		/// Otherwise, break before and after newlines.
		/// </summary>
		WB3b,
		/// <summary>
		/// Ignore format and extend characters.
		/// </summary>
		WB4,
		/// <summary>
		/// Do not break between most letters.
		/// </summary>
		WB5,
		/// <summary>
		/// Break between apostrophe and vowels (French, Italian).
		/// </summary>
		WB5a,
		/// <summary>
		/// Do not break across certain punctuation.
		/// </summary>
		WB6,
		/// <summary>
		/// Do not break across certain punctuation.
		/// </summary>
		WB7,
		/// <summary>
		/// Do not break within sequences of digits, or digits adjacent to letters.
		/// </summary>
		WB8,
		/// <summary>
		/// Do not break within sequences of digits, or digits adjacent to letters.
		/// </summary>
		WB9,
		/// <summary>
		/// Do not break within sequences of digits, or digits adjacent to letters.
		/// </summary>
		WB10,
		/// <summary>
		/// Do not break within sequences, such as "3.2" or "3,456.789".
		/// </summary>
		WB11,
		/// <summary>
		/// Do not break within sequences, such as "3.2" or "3,456.789".
		/// </summary>
		WB12,
		/// <summary>
		/// Do not break between Katakana.
		/// </summary>
		WB13,
		/// <summary>
		/// Do not break from extenders.
		/// </summary>
		WB13a,
		/// <summary>
		/// Do not break from extenders.
		/// </summary>
		WB13b,
		/// <summary>
		/// Otherwise, break everywhere.
		/// </summary>
		WB14
	}
}
