using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GmxVCount.TextBlocks
{
	/// <summary>
	/// Represents a character within a textblock (i.e. not a word 
	/// break).
	/// </summary>
	public class Character : TextBlockParticle
	{
		/// <summary>
		/// Creates an instance of the Character text block particle.
		/// </summary>
		/// <param name="value">The character to add.</param>
		public Character(char value)
		{
			this.Value = value;
			this.Rule = WordBoundaryRule.None;
		}

		/// <summary>
		/// Creates an instance of the Character text block particle.
		/// </summary>
		/// <param name="value">The character to add.</param>
		/// <param name="rule">The rule from TR-29 which was matched to not force a word break.</param>
		public Character(char value, WordBoundaryRule rule)
		{
			this.Value = value;
			this.Rule = rule;
		}

		/// <summary>
		/// The character within the text block.
		/// </summary>
		public char Value { get; set; }

		/// <summary>
		/// The rule from TR-29 which was matched to not force a word break.
		/// </summary>
		public WordBoundaryRule Rule { get; set; }
	}
}
