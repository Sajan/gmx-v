using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GmxVCount.WordCounter;
using System.IO;

namespace WordCount
{
	/// <summary>
	/// A program which accepts an input XLIFF document and outputs a GMX/V word count to the 
	/// console.
	/// </summary>
	class Program
	{
		/// <summary>
		/// The main entry point of the application.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			string fileName = string.Join(" ", args);

			if (string.IsNullOrEmpty(fileName) || fileName == "-?" || fileName == "/?")
			{
				WriteConsoleParameters();
			}
			else
			{
				if (!File.Exists(fileName))
				{
					Console.WriteLine("The filename \"{0}\" does not exist.", fileName);
					Console.WriteLine();
					WriteConsoleParameters();
				}
				else
				{
					CountWords(fileName);
				}
			}
		}

		/// <summary>
		/// Counts the words in the input XLIFF document and renders the resultant
		/// GMX/V file to the screen.
		/// </summary>
		/// <param name="fileName">The XLIFF document to count.</param>
		private static void CountWords(string fileName)
		{
			using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				// There could be multiple file elements within in the XLIFF document.
				foreach (WordCountValueCollection wordCount in GmxVWordCounter.CountWords(fs))
				{
					Console.WriteLine(wordCount.ToGmxV().ToString());
				}
			}
		}

		/// <summary>
		/// Writes the available command line parameters to the console.
		/// </summary>
		private static void WriteConsoleParameters()
		{
			Console.WriteLine("GMX/V Word Count");
			Console.WriteLine();
			Console.WriteLine("This application provides a GMX/V word count for a given ");
			Console.WriteLine("input XLIFF document.");
			Console.WriteLine();
			Console.WriteLine("Usage Examples: ");
			Console.WriteLine("  WordCount.exe C:\\My Folders\\MyXliff.xlf");
			Console.WriteLine("  WordCount.exe chinese.xlf");
			Console.WriteLine();
			Console.WriteLine("For more information visit: https://code.google.com/p/gmx-v/");
		}
	}
}
