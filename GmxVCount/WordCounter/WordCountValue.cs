using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GmxVCount.TextBlocks;
using GmxVCount.TR29;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Globalization;
using System.IO;
using System.Xml;

namespace GmxVCount.WordCounter
{
	/// <summary>
	/// Represents a Word Count based on word boundaries
	/// as specified by TR29.
	/// </summary>
	public class WordCountValue
	{
		/// <summary>
		/// Creates an instance of the WordCountValue type.
		/// </summary>
		/// <param name="sourceCulture">The Source Culture of the Word Count.</param>
		public WordCountValue(Language sourceCulture)
		{
			this.Data = new TextBlock();
			this.SourceCulture = sourceCulture;
		}

		/// <summary>
		/// Retrieves the Character Count from the TextBlock data.
		/// </summary>
		public int CharacterCount
		{
			get
			{
				return this.Data.Text.Length;
			}
		}

		/// <summary>
		/// Retrieves the Word Count from the TextBlock data.
		/// </summary>
		public int WordCount
		{
			get
			{
				return this.Data.WordCount;
			}
		}

		/// <summary>
		/// The underlying data used to determine the word count.
		/// </summary>
		public TextBlock Data { get; set; }

		/// <summary>
		/// Returns a minimal GMX/V document containing the Total Word Count and Total
		/// Character Count.
		/// </summary>
		/// <param name="sourceCulture">The source culture of the analysis.</param>
		/// <param name="characterCount">The character count of the analysis.</param>
		/// <param name="wordCount">The word count of the analysis.</param>
		/// <returns>An XML document containing a GMX/V document.</returns>
		public static XDocument ToGmxV(Language sourceCulture, int wordCount, int characterCount)
		{
			XNamespace ns = "urn:lisa-metrics-tags";

			StringBuilder sb = new StringBuilder();

			// If the culture is "Logographic", then set the character count to zero.
			if (sourceCulture.IsLogographicLanguage)
			{
				wordCount = 0;
			}

			// Add the metrics root element.
			sb.Append(string.Format(CultureInfo.InvariantCulture,
				"<?xml version=\"1.0\"?>" +
				"<metrics xmlns=\"urn:lisa-metrics-tags\" version=\"1.0\" source-language=\"{0}\" tool-name=\"Gould Tech Solutions GMX/V Word Counter\" tool-version=\"1.0\">",
				sourceCulture.IsoCode));

			// Add the stage.
			sb.Append(string.Format(CultureInfo.InvariantCulture,
				"<stage phase=\"initial\" date=\"{0}\">",
				DateTime.UtcNow.ToString("yyyMMddThh:mm:ssZ")));

			// Add the count group
			sb.Append(string.Format(CultureInfo.InvariantCulture,
				"<count-group name=\"verifiable\"><count type=\"TotalWordCount\" value=\"{0}\"/>" +
				"<count type=\"TotalCharacterCount\" value=\"{1}\"/></count-group>", wordCount, characterCount));

			// Close the stage
			sb.Append("</stage>");
			
			// Close the metrics.
			sb.Append("</metrics>");

			return XDocument.Parse(sb.ToString());
		}

		/// <summary>
		/// Returns a minimal GMX/V document containing the Total Word Count and Total
		/// Character Count.
		/// </summary>
		/// <returns>An XML document containing a GMX/V document.</returns>
		public XDocument ToGmxV()
		{
			return ToGmxV(this.SourceCulture, this.WordCount, this.CharacterCount);
		}

		/// <summary>
		/// The Source Culture of the Word Count.
		/// </summary>
		public Language SourceCulture
		{
			get;
			private set;
		}
	}
}
