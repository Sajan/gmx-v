using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GmxVCount.TextBlocks
{
	/// <summary>
	/// Represents a block of text which has been word counted.
	/// </summary>
	public class TextBlock : List<TextBlockParticle>
	{
		/// <summary>
		/// Returns the text.
		/// </summary>
		public string Text
		{
			get
			{
				return new String(this.OfType<Character>().Select(c => c.Value).ToArray());
			}
		}

		/// <summary>
		/// Retrieves the Word Count from the TextBlock data.
		/// </summary>
		public int WordCount
		{
			get
			{
				// Proximity tests in searching determines whether, for example, “quick” is within 
				// three words of “fox”. That is done with the above boundaries by ignoring any 
				// words that do not contain a letter, as in Figure 2. 
				int wordCount = 0;
				foreach (string w in this.Words)
				{
					if (NonspaceRegularExpression.IsMatch(w))
					{
						wordCount++;
					}
				}

				return wordCount;
			}
		}

		/// <summary>
		/// Matches any letter or number.  See http://msdn.microsoft.com/en-us/library/20bw873z.aspx#Y16809 
		/// </summary>
		public static Regex NonspaceRegularExpression = new Regex(@"[\p{L}\p{N}]", RegexOptions.Compiled);

		/// <summary>
		/// Iterates through the TextBlock data and splits the text into words.
		/// </summary>
		public List<string> Words
		{
			get
			{
				var words = new List<string>();
				var word = new List<char>();

				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] is WordBoundary)
					{
						// End the current word.
						words.Add(new string(word.ToArray()));
						word = new List<char>();
					}
					else
					{
						// Must be a character.
						word.Add(((Character)this[i]).Value);
					}
				}

				return words;
			}
		}

		/// <summary>
		/// Returns the text, but showing the counted and non-counted
		/// word boundaries within it.
		/// </summary>
		public string TextWithWords
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				// Proximity tests in searching determines whether, for example, “quick” is within 
				// three words of “fox”. That is done with the above boundaries by ignoring any 
				// words that do not contain a letter, as in Figure 2. 
				foreach (string w in this.Words)
				{
					if (NonspaceRegularExpression.IsMatch(w))
					{
						// This is a counted words.
						sb.Append("|" + w + "|" );
					}
					else
					{
						// This is a noncounted word.
						sb.Append(w + " ");
					}
				}

				return sb.ToString();
			}
		}

		/// <summary>
		/// Returns the text, but showing the word boundaries within it.
		/// </summary>
		public string TextWithBoundaries
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				foreach (TextBlockParticle p in this)
				{
					if (p is Character)
					{
						sb.Append(((Character)p).Value);

						if (((Character)p).Rule != WordBoundaryRule.None)
						{
							//sb.Append("{" + ((Character)p).Rule + "}");
						}
					}
					else if (p is WordBoundary)
					{
						sb.Append("|");
					}
				}

				return sb.ToString();
			}
		}
	}
}
