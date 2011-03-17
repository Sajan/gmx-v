using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace GmxVCount.WordCounter.XmlUtilities
{
	/// <summary>
	/// A class containing helper methods for loading XML from Streams.
	/// </summary>
	public static class XmlHelper
	{
		/// <summary>
		/// Creates an XmlReader with default settings for this application, based on the StreamReader
		/// passed in.
		/// </summary>
		/// <param name="sr">The stream to read from.</param>
		/// <param name="defaultNamespace">The default namespace to use, or an empty string.</param>
		/// <returns>An XmlTextReader.</returns>
		public static XmlReader CreateReader(StreamReader sr, string defaultNamespace)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ValidationType = ValidationType.None;
			settings.IgnoreWhitespace = false;
			settings.DtdProcessing = DtdProcessing.Ignore;
			settings.XmlResolver = null;

			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(new SimpleNameTable());
			if (!string.IsNullOrEmpty(defaultNamespace))
			{
				namespaceManager.AddNamespace(string.Empty, defaultNamespace);
			}

			XmlParserContext context = new XmlParserContext(null, namespaceManager, null, XmlSpace.Default);

			return XmlReader.Create(sr, settings, context);
		}
	}
}
