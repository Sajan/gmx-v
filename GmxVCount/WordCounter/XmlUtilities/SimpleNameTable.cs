using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GmxVCount.WordCounter.XmlUtilities
{
	/// <summary>
	/// A class used to create a very simple XmlNameTable.  Example taken from:
	/// http://stackoverflow.com/questions/879728/can-i-use-predefined-namespaces-when-loading-an-xdocument
	/// </summary>
	public class SimpleNameTable : XmlNameTable
	{
		List<string> cache = new List<string>();

		/// <summary>
		/// Adds data to the cache.
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public override string Add(string array)
		{
			string found = cache.Find(s => s == array);
			if (found != null) return found;
			cache.Add(array);
			return array;
		}

		/// <summary>
		/// Adds data to the cache.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override string Add(char[] array, int offset, int length)
		{
			return this.Add(new string(array, offset, length));
		}

		/// <summary>
		/// Gets an item from the Simple Name Table Cache.
		/// </summary>
		/// <param name="array"></param>
		/// <returns></returns>
		public override string Get(string array)
		{
			return cache.Find(s => s == array);
		}

		/// <summary>
		/// Gets a string from the characters.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override string Get(char[] array, int offset, int length)
		{
			return Get(new string(array, offset, length));
		}
	}
}
