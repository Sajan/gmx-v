using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GmxVCount.TR29;
using GmxVCount.TextBlocks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.IO;
using GmxVCount.WordCounter.XmlUtilities;

namespace GmxVCount.WordCounter
{
	/// <summary>
	/// A class which provides a GMX/V compliant word count.
	/// </summary>
	public static class GmxVWordCounter
	{
		#region XLIFF Handling

		/// <summary>
		/// Counts the words in a given XLIFF document.
		/// </summary>
		/// <param name="xliffDocumentStream">The XLIFF document to count.</param>
		/// <returns>An array of WordCountValues, one per file element within the XLIFF file.</returns>
		public static WordCountValueCollection[] CountWords(Stream xliffDocumentStream)
		{
			using (var sr = new StreamReader(xliffDocumentStream))
			{
				using (var xmlReader = XmlHelper.CreateReader(sr, "urn:oasis:names:tc:xliff:document:1.2"))
				{
					return CountWords(XDocument.Load(xmlReader));
				}
			}
		}

		/// <summary>
		/// Counts the words in a given XLIFF document.
		/// </summary>
		/// <param name="xliffDocument">The XLIFF document to count.</param>
		/// <returns>Metrics.</returns>
		public static WordCountValueCollection[] CountWords(XDocument xliffDocument)
		{
			var returnValue = new List<WordCountValueCollection>();

			XNamespace defaultNamespace = xliffDocument.Root.Name.Namespace;
			var namespaceManager = new XmlNamespaceManager(xliffDocument.CreateReader().NameTable);
			namespaceManager.AddNamespace("xlf", "urn:oasis:names:tc:xliff:document:1.2");

			if (xliffDocument.Root.Name != XName.Get("xliff", defaultNamespace.NamespaceName))
			{
				throw new ArgumentException("The provided XDocument is not a valid XLIFF document.");
			}

			// An XLIFF file can contain multiple translation files.
			foreach (XElement file in xliffDocument.XPathSelectElements("/xlf:xliff/xlf:file", namespaceManager))
			{
				returnValue.Add(AnalyseXliffFileElement(namespaceManager, file));
			}

			return returnValue.ToArray();
		}

		/// <summary>
		/// Analyses an XLIFF file element to determine the word counts.
		/// </summary>
		/// <param name="file">A file element from an XLIFF file.</param>
		/// <returns>A collection of Word Counts (one word count per translation unit).</returns>
		public static WordCountValueCollection AnalyseXliffFileElement(XElement file)
		{
			var namespaceManager = new XmlNamespaceManager(file.CreateReader().NameTable);
			namespaceManager.AddNamespace("xlf", "urn:oasis:names:tc:xliff:document:1.2");

			return AnalyseXliffFileElement(namespaceManager, file);
		}

		/// <summary>
		/// Analyses an XLIFF file element to determine the word counts.
		/// </summary>
		/// <param name="namespaceManager">The namespaceManager used to identify XML namespaces.</param>
		/// <param name="file">A file element from an XLIFF file.</param>
		/// <returns>A collection of Word Counts (one word count per translation unit).</returns>
		private static WordCountValueCollection AnalyseXliffFileElement(XmlNamespaceManager namespaceManager, XElement file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("The file element cannot be null.", "file");
			}

			if (file.Name.LocalName != "file")
			{
				throw new ArgumentException(
					string.Format(CultureInfo.InvariantCulture, "The provided file element is not valid.  It shold be named \"file\" not {0}.", file.Name.LocalName), 
					"file");
			}

			var returnValue = new WordCountValueCollection();

			// We can retrieve the source language code for the document.
			string sourceLanguageCode = file.Attribute(XName.Get("source-language")).Value;

			if (string.IsNullOrEmpty(sourceLanguageCode))
			{
				throw new ArgumentException("One of the files within the XLIFF document did not contain a source-language attribute.");
			}

			// Grab the source culture.
			var sourceCulture = new Language(sourceLanguageCode);

			// Iterate through the Translation Units and count the text.
			foreach (XElement translationUnit in file.XPathSelectElements("//xlf:trans-unit", namespaceManager))
			{
				// Check that this translation unit is translatable.				
				if (IsTranslationUnitTranslatable(translationUnit))
				{
					XElement source = translationUnit.XPathSelectElement("xlf:source", namespaceManager);

					string text = ExtractTextFromChildNodes(source.Nodes());

					// Process white space according to section 2.7 of the GMX/V specification.
					XAttribute xmlSpaceAttribute = source.Parent.Attribute(XName.Get("{http://www.w3.org/XML/1998/namespace}space"));
					bool clearWhiteSpace = true;

					if (xmlSpaceAttribute != null && xmlSpaceAttribute.Value == "preserve")
					{
						clearWhiteSpace = false;
					}

					if (clearWhiteSpace)
					{
						text = Regex.Replace(text, "\\s+", " ");
					}

					returnValue.Add(CountWords(text, sourceCulture));
				}
			}

			return returnValue;
		}

		private static bool IsTranslationUnitTranslatable(XElement translationUnit)
		{
			if (translationUnit.Name.LocalName != "trans-unit")
			{
				throw new ArgumentException("The XElement passed was not a translation unit (trans-unit) element.", "translationUnit");
			}

			bool isTranslatable = true;

			if (!IsTranslatable(translationUnit))
			{
				isTranslatable = false;
			}
			else
			{
				// Check any parent "group" elements to make sure that the whole 
				// group hasn't been set to be untranslateable.
				XElement parent = translationUnit.Parent;

				while (parent != null)
				{
					if (parent.Name.LocalName == "group")
					{
						if (!IsTranslatable(parent))
						{
							isTranslatable = false;
							break;
						}
					}

					parent = parent.Parent;
				}
			}

			return isTranslatable;
		}

		private static bool IsTranslatable(XElement e)
		{
			XAttribute attribute = e.Attribute(XName.Get("translate"));

			bool returnValue = true;

			if (attribute != null)
			{
				returnValue = !(attribute.Value == "no");
			}
			
			return returnValue;
		}
		
		/// <summary>
		/// Extracts text from an XLIFF inline element, subflow or marker.
		/// </summary>
		/// <param name="element">The XLIFF element.</param>
		/// <returns>Text extracted from text-containing nodes.</returns>
		private static string ExtractText(XElement element)
		{
			var sb = new StringBuilder();

			switch (element.Name.LocalName)
			{
				case "x": // Generic Placeholder
				case "bx": // Begin Paired Placeholder
				case "ex": // End paired placeholder
					// Add the equiv-text
					sb.Append(ExtractEquivalentText(element));
					break;
				case "bpt": // Begin Paired Tag
				case "ept": // End Paired Tag
				case "ph": // Placeholder
				case "it": // Isolated tag
					// Add any equivalent text.
					sb.Append(ExtractEquivalentText(element));
					// bpt, ept, ph and it can also contain sub elements, so we need to
					// extract these.
					var subflows = element.Elements(XName.Get("sub", "xlf"));
					sb.Append(ExtractTextFromChildNodes(subflows));
					break;
				case "mrk":
				case "sub":
				case "g": // Generic Group Placeholder
					// mrk, and subs can contain other elements and text.
					sb.Append(ExtractTextFromChildNodes(element.Nodes()));
					break;
				default:
					throw new ArgumentException(string.Format("The element with name \"{0}\" was not expected in an XLIFF document.", element.Name.ToString()));
			}

			return sb.ToString();
		}

		/// <summary>
		/// Extracts Equivalent Text from the inline elements, where available.
		/// </summary>
		/// <param name="e">The g, x, bx, ex, bpt, ept, ph or it element.</param>
		/// <returns>Equivalent Text from the inline elements, where available, or string.Empty.</returns>
		private static string ExtractEquivalentText(XElement e)
		{
			string returnValue = string.Empty;

			XAttribute attribute = e.Attribute(XName.Get("equiv-text"));

			if (attribute != null)
			{
				returnValue = attribute.Value;
			}

			return returnValue;
		}

		/// <summary>
		/// Extracts text nodes from Child Nodes of an XLIFF document.
		/// </summary>
		/// <param name="childNodes">The XLIFF child nodes to test.</param>
		private static string ExtractTextFromChildNodes(IEnumerable<XNode> childNodes)
		{
			StringBuilder sb = new StringBuilder();

			foreach (XNode node in childNodes)
			{
				if (node is XText)
				{
					sb.Append(((XText)node).Value);
				}
				else if (node is XElement)
				{
					sb.Append(ExtractText((XElement)node));
				}
			}

			return sb.ToString();
		}
		#endregion

		/// <summary>
		/// Counts Words according to the rules set out at http://www.unicode.org/reports/tr29/#Word_Boundaries
		/// </summary>
		/// <param name="text">The text to count.</param>
		/// <param name="culture">The culture of the word count.</param>
		/// <returns></returns>
		public static WordCountValue CountWords(string text, CultureInfo culture)
		{
			return CountWords(text, new Language(culture.Name));
		}

		/// <summary>
		/// Counts Words according to the rules set out at http://www.unicode.org/reports/tr29/#Word_Boundaries
		/// </summary>
		/// <param name="text">The text to count.</param>
		/// <param name="culture">The culture of the word count.</param>
		/// <returns></returns>
		public static WordCountValue CountWords(string text, Language culture)
		{
			var wordCount = new WordCountValue(culture);

			for (int index = 0; index < text.Length; index++)
			{
				char previous = index - 1 >= 0 ? text[index - 1] : char.MinValue;
				char current = text[index];
				char next = index < text.Length - 1 ? text[index + 1] : char.MinValue;
				char nextNext = index < text.Length - 2 ? text[index + 2] : char.MinValue;

				// Break at the start and end of text.
				// WB1
				if (index == 0)
				{
					wordCount.Data.Add(new WordBoundary(WordBoundaryRule.WB1));
					//wordCount.Data.Add(new Character(current));
					//continue;
				}

				// WB2
				if (index == text.Length - 1)
				{
					wordCount.Data.Add(new Character(current));
					wordCount.Data.Add(new WordBoundary(WordBoundaryRule.WB2));
					continue;
				}

				// Do not break within CRLF
				// WB3
				if (
					(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.CR, previous) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.LF, current))
					||
					(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.CR, current) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.LF, next))
					)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB3));
					continue;
				}

				// Otherwise break before and after Newlines (including CR and LF)
				// WB3a
				if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Newline, previous) || TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.CR, previous) || TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.LF, previous))
				{
					wordCount.Data.Add(new WordBoundary(WordBoundaryRule.WB3a));
					wordCount.Data.Add(new Character(current));
					continue;
				}

				// WB3b is not relevant.

				// Ignore Format and Extend characters, except when they appear at the beginning of a region of text.
				// (See Section 6.2, Replacing Ignore Rules.)
				// WB4 - Ignore Format and Extend characters is not relevant.
				
				// Define a variable for these two, the search is done several times from here onwards and it's the
				// slowest search (through 18,000 characters).
				bool currentIsALetter = TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ALetter, current);
				bool nextIsALetter = TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ALetter, next);

				// Do not break between most letters.
				// WB5
				if (currentIsALetter && nextIsALetter)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB5));
					continue;
				}

				// The rules for French and Italian  are slightly different.

				/*
				 * In addition Unicode TR 29 Section 4 provides an optional rule for the apostrophe 
				 * character which relates to French and Italian usage such as "l'objectif". This rule 
				 * known as "Break between apostrophe and vowels (French, Italian)" must also be applied 
				 * for GMX-V. Apostrophe includes U+0027 (') APOSTROPHE and U+2019 (’) RIGHT SINGLE 
				 * QUOTATION MARK (curly apostrophe).
				 */
				Language rootCulture = culture.Root;

				if (rootCulture.IsoCode == "fr" || rootCulture.IsoCode == "it")
				{
					// WB5a
					if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Apostrophe, current) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Vowel, next))
					{
						wordCount.Data.Add(new Character(current));
						wordCount.Data.Add(new WordBoundary(WordBoundaryRule.WB5a));
						continue;
					}
				}

				/* The GMX/V specification states that hyphens are not word break characters (see section
				 * 2.8 
				 */
				if (currentIsALetter && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Hyphen, next)
					|| TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Hyphen, current) && nextIsALetter)
				{
					wordCount.Data.Add(new Character(current));
					continue;
				}

				// Do not break letters across certain punctuation.
				// WB6
				if (currentIsALetter &&
					(
						(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidLetter, next) || TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidNumLet, next)) &&
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ALetter, nextNext)
					))
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB6));
					continue;
				}

				// WB7
				if (
					TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ALetter, previous) &&
					(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidLetter, current) || TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidNumLet, current))
					&& nextIsALetter
					)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB7));
					continue;
				}

				// Do not break within sequences of digits, or digits adjacent to letters (“3a”, or “A3”).
				// WB8
				if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, previous) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, current) ||
					TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, current) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, next))
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB8));
					continue;
				}

				// WB9
				if (currentIsALetter && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, next))
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB9));
					continue;
				}

				// WB10
				if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, current) && nextIsALetter)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB10));
					continue;
				}

				// Do not break within sequences, such as "3.2" or "3,456.789".
				// WB11
				if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, previous) &&
					(
						(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidNum, current) || TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidNumLet, current)) &&
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, next)
					))
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB11));
					continue;
				}

				// WB12
				if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, current) &&
					(
						(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidNum, next) || TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.MidNumLet, next)) &&
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, nextNext)
					))
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB11));
					continue;
				}

				// Do not break between Katakana.
				// WB13
				if (
					(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Katakana, previous) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Katakana, current))
					||
					(TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Katakana, current) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Katakana, next))
					)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB13));
					continue;
				}

				// Do not break from extenders.
				// WB13a.
				if (
					(
						currentIsALetter ||
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, current) ||
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Katakana, current) ||
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ExtendNumLet, current)
					) && TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ExtendNumLet, next)
					)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB13a));
					continue;
				}

				// WB13b.
				if (TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.ExtendNumLet, current) &&
					(
						nextIsALetter ||
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Numeric, next) ||
						TextSegmentationHelper.IsInCategory(UnicodeWordBreakCategory.Katakana, next)
					)
				)
				{
					wordCount.Data.Add(new Character(current, WordBoundaryRule.WB13b));
					continue;
				}

				// WB14 - Break everywhere else.
				wordCount.Data.Add(new Character(current));
				wordCount.Data.Add(new WordBoundary(WordBoundaryRule.WB14));
			}

			return wordCount;
		}
	}
}
