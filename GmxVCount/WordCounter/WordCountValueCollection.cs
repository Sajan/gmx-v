using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace GmxVCount.WordCounter
{
	/// <summary>
	/// A collection of individual word counts.
	/// </summary>
	public class WordCountValueCollection : List<WordCountValue>
	{
		/// <summary>
		/// The total number of characters in the Word Count Value collection.
		/// </summary>
		public int TotalCharacterCount
		{
			get
			{
				return this.Sum(s => s.CharacterCount);
			}
		}

		/// <summary>
		/// The total number of words in the Word Count Value collection.
		/// </summary>
		public int TotalWordCount
		{
			get
			{
				return this.Sum(s => s.WordCount);
			}
		}

		/// <summary>
		/// Returns a minimal GMX/V document containing the Total Word Count and Total
		/// Character Count.
		/// </summary>
		/// <returns>An list of XML documents containing GMX/V documents (one per source language).</returns>
		public XDocument ToGmxV()
		{
			if (this.Select(wcv => wcv.SourceCulture).Distinct().Count() > 1)
			{
				throw new InvalidOperationException("It is not possible to combine word counts into a GMX/V result " +
					"if more than one culture is present in the collection.");
			}

			WordCountValue wordCountValue = this.FirstOrDefault();

			if (wordCountValue == null)
			{
				throw new InvalidOperationException("It is not possible to create a GMX/V word count if there are no " +
					"word counts in the collection.");
			}

			int totalCharacterCount = this.TotalCharacterCount;
			int totalWordCount = this.TotalWordCount;

			return WordCountValue.ToGmxV(wordCountValue.SourceCulture, totalWordCount, totalCharacterCount);
		}
	}
}
