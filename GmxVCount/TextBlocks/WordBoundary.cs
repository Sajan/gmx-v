using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmxVCount.TextBlocks
{
	/// <summary>
	/// Represents a word boundary within a text block.
	/// </summary>
	public class WordBoundary : TextBlockParticle
	{
		/// <summary>
		/// Creates an instance of a Word boundary.
		/// </summary>
		/// <param name="rule">The rule from TR-29 which was matched to force the word break.</param>
		public WordBoundary(WordBoundaryRule rule)
		{
			this.Rule = rule;
		}

		/// <summary>
		/// The rule from TR-29 which was matched to force the word break.
		/// </summary>
		public WordBoundaryRule Rule { get; set; }
	}
}
