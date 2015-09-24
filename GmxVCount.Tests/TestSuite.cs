using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GmxVCount.WordCounter;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using NUnit.Framework;
using System.Diagnostics;

namespace GmxVCount.Tests
{
	[TestFixture]
	public class TestSuite
	{
		[TestCase("output.xlf", 175, 992)]
		[TestCase("xmlspace.xlf", 28, 150)]
		[TestCase("translatable.xlf", 6, 28)]
		[TestCase("inwordformatting.xlf", 3, 24)]
		[TestCase("chinese.xlf", 0, 21)]
		[TestCase("equivalenttext.xlf", 9, 49)]
		[TestCase("whitespace.xlf", 12, 111)]
		public void TestXliffWordCount(string fileName, int expectedWordCount, int expectedCharacterCount)
		{
			// Tell the user what we're doing.
			Debug.WriteLine("Testing XLIFF analysis.");

			// Load the Test XLIFF document from the Assembly and 
			// count the words in the XLIFF document.
			WordCountValueCollection metrics = null;

			using (Stream stream = typeof(TestSuite).Assembly.GetManifestResourceStream("GmxVCount.Tests.TestFiles." + fileName))
			{
				// There's only one XLIFF file element in each of the test files.
				metrics = GmxVWordCounter.CountWords(stream).First();
			}

			// We should now have a single GMX/V document.
			XDocument wordCount = metrics.ToGmxV();

			// Read back the values.
			XElement stage = wordCount.Root.Element(XName.Get("stage", "urn:lisa-metrics-tags"));
			XElement countGroup = stage.Element(XName.Get("count-group", "urn:lisa-metrics-tags"));
			XName count = XName.Get("count", "urn:lisa-metrics-tags");
			XElement totalWordcount = countGroup.Elements(count).Where(c => c.Attribute(XName.Get("type")).Value == "TotalWordCount").First();
			XElement totalCharacter = countGroup.Elements(count).Where(c => c.Attribute(XName.Get("type")).Value == "TotalCharacterCount").First();

			// Test the word count
			int xliffWordCount = int.Parse(totalWordcount.Attribute("value").Value);

			Assert.That(xliffWordCount, Is.EqualTo(expectedWordCount),
				string.Format(
					CultureInfo.InvariantCulture,
					"The XLIFF word count was expected to be {0} but was {1}.",
					expectedWordCount,
					xliffWordCount));

			// Check the character count.
			int xliffCharacterCount = int.Parse(totalCharacter.Attribute("value").Value);

			Assert.That(xliffCharacterCount, Is.EqualTo(expectedCharacterCount),
				string.Format(
					CultureInfo.InvariantCulture,
					"The XLIFF character count was expected to be {0} but was {1}.",
					expectedCharacterCount,
					xliffCharacterCount));

			Debug.WriteLine(wordCount.ToString());
		}

		[Test]
		public void WhitespaceTest()
		{
			Test("en-US", "Test" + (char)0x00A0 + "non-breaking" + (char)0x2007 + "spaces" + (char)0x202F + "test.", 4);
		}

		// Simple tests.
		[TestCase("en-GB", "This is a test.", 4)]
		[TestCase("en-GB", "This sentence has a word count of 9 words.", 9)]
		[TestCase("en-GB", "This sentence/text unit has a word count of 11 words.", 11)]
		[TestCase("en-GB", "This is a test", 4)]

		// Test for hyphens
		// GMX/V doesn't break for hyphens.
		[TestCase("en-GB", "This-is-a-single-word", 1)]

		// Chinese test.
		[TestCase("zh-CN", "这是汉语。", 4)]

		// Do not break letters across certain punctuation.
		[TestCase("en-GB", "I can’t find that in L’Observatore Romano.", 7)]
		// Special rule for French.
		[TestCase("fr-FR", "C’est l’oiseau bleu.", 5)]

		// Duplicate spaces don't count.
		[TestCase("en-GB", "Two  spaces   three   spaces", 4)]

		//// Specific rules
		[TestCase("en-GB", "WB1", 1)]
		[TestCase("en-GB", "WB2", 1)]
		[TestCase("en-GB", "WB3: This\r\nis", 3)]
		// WB4 doesn't affect the code at this point, because the .Net code has already
		// converted any combinations into a single grapheme.
		[TestCase("en-GB", "WB5: shouldn't break between lEtTeRs.", 5)]
		// In TR29, the default behaviour is to break 'Italian-American' into two words.
		// However, GMX/V overrides this behaviour and counts this as a single word.
		[TestCase("en-GB", "WB6: Hyphens-don't-break.", 2)]
		[TestCase("en-GB", "WB6: a:a", 2)]
		[TestCase("en-GB", "WB7: a'a", 2)]
		[TestCase("en-GB", "WB8: 12345 - Don't break across sequences of digits.", 8)]
		[TestCase("en-GB", "WB9: This mixes numbers and letters: 123DB.", 7)]
		[TestCase("en-GB", "WB9: This mixes numbers and letters: A1c.", 7)]
		[TestCase("en-GB", "WB10: This mixes letters and numbers: DB123.", 7)]
		// WB11 and 12 tests
		[TestCase("en-GB", "3.2", 1)]
		[TestCase("en-GB", "10,000.00", 1)]
		[TestCase("en-GB", "WB12: Do not break within sequences, such as “3.2” or “3,456.789”.", 11)]
		// WB13: Katakana tests.
		[TestCase("ja-JP", "私はピアノが出来ない", 7)] // ピアノ is katakana, the others are hirigana and kanji
		[TestCase("ja-JP", "俺はデジタル音楽が好き", 7)] // デジタル is katakana
		[TestCase("ja-JP", "オフ会する訳がない！", 7)] // オフ会 is a single word, comprised of 2 katakana and a kanji
		// WB13a: Don't break on extenders.
		[TestCase("en-GB", "WB13a: a123__ extra extra.", 4)]
		[TestCase("en-GB", "a3__aa some more.", 3)]
		// WB13b: Don't break on extenders.
		[TestCase("en-GB", "WB13b: __variableName is here.", 4)]
		// WB14 is excercised by there being any words counted at all.
		public void Test(string culture, string text, int expectedWordCount)
		{
			var language = new Language(culture);

			WordCountValue words = GmxVWordCounter.CountWords(text, language);

			// Display the total words.
			Debug.WriteLine("Data: " + words.Data.Text);
			Debug.WriteLine("Data: " + words.Data.TextWithBoundaries);

			int actual = words.WordCount;

			Assert.That(actual, Is.EqualTo(expectedWordCount), string.Format(CultureInfo.InvariantCulture, "The test didn't pass.  Expected words: {0}.  Actual words: {1}.", expectedWordCount, actual));
		}
	}
}