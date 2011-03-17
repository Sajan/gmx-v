using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GmxVCount
{
	/// <summary>
	/// Represents a given language code.
	/// </summary>
	public class Language
	{
		/// <summary>
		/// Remains protected for serialization.
		/// </summary>
		protected Language()
		{
		}

		/// <summary>
		/// Creates an instance of the Language class.
		/// </summary>
		/// <param name="isoCode"></param>
		public Language(string isoCode)
		{
			this.IsoCode = isoCode;
		}

		private string _isoCode;

		/// <summary>
		/// Gets or sets the ISO code for the given language.
		/// </summary>
		public string IsoCode
		{
			get
			{
				return _isoCode;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException("The IsoCode property cannot be null or empty.", "value");
				}

				if (!IsCultureCodeValid(value))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The IsoCode property must be formatted correctly, e.g. \"en\" or \"en-GB\".  \"{0}\" is not a valid ISO code.", value), "value");
				}

				_isoCode = value;
			}
		}

		/// <summary>
		/// Checks whether a given culture code is valid.
		/// </summary>
		/// <param name="isoCode">The ISO code to validate.</param>
		/// <returns>True if the ISO code is valid, false if it isn't.</returns>
		public static bool IsCultureCodeValid(string isoCode)
		{
			return Regex.IsMatch(isoCode, "^[A-Za-z]+(-?[A-Za-z0-9]+)*$");
		}

		/// <summary>
		/// Retrieves the parent language for this culture code. 
		/// </summary>
		public Language Parent
		{
			get
			{
				string[] segments = this.IsoCode.Split('-');
				string parentIsoCode = this.IsoCode;

				if (segments.Length > 1)
				{
					parentIsoCode = segments[segments.Length - 2];
				}

				return new Language(parentIsoCode);
			}
		}

		/// <summary>
		/// Walks up the heirarchy of cultures to find the root culture.  For instance,
		/// if fr-FR is passed in, the method will return "fr".
		/// </summary>
		/// <returns>The parent culture.</returns>
		public Language Root
		{
			get
			{
				Language parentCulture = new Language(this.IsoCode);

				while (parentCulture.Parent != parentCulture)
				{
					parentCulture = parentCulture.Parent;
				}

				return parentCulture;
			}
		}

		/// <summary>
		/// Returns true when the current language is considered to be 
		/// logographic.
		/// </summary>
		public bool IsLogographicLanguage
		{
			get
			{
				bool isLogographicLanguage = false;

				switch(this.Root.IsoCode.ToLowerInvariant())
				{
					case "th": // Thai
					case "lo": // Lao
					case "km": // Khmer
					case "my": // Myanmar
					case "zh": // Chinese
					case "ja": // Japanese
					case "ko": // Korean
						isLogographicLanguage = true;
						break;
				}

				return isLogographicLanguage;
			}
		}

		/// <summary>
		/// Overrides the behaviour of the equality operator to provide 
		/// natural usage of the type.  Reference types are compared based
		/// on their memory locations, but we want to compare the Language
		/// type base on its ISO code property.
		/// </summary>
		/// <param name="x">Language A</param>
		/// <param name="y">Language B</param>
		/// <returns>True if the Languages are equal, false if not.</returns>
		public static bool operator ==(Language x, Language y)
		{
			return x.Equals(y);
		}

		/// <summary>
		/// Overrides the behaviour of the inequality operator to provide 
		/// natural usage of the type.  Reference types are compared based
		/// on their memory locations, but we want to compare the Language
		/// type base on its ISO code property.
		/// </summary>
		/// <param name="x">Language A</param>
		/// <param name="y">Language B</param>
		/// <returns>True if the Languages are inequal, false if not.</returns>
		public static bool operator !=(Language x, Language y)
		{
			return !x.Equals(y);
		}

		/// <summary>
		/// Determines whether the input object is equal to this object.
		/// </summary>
		/// <param name="obj">The object to compare against.</param>
		/// <returns>True if the objects are equal, false if not.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Language)
			{
				string isoCode = ((Language)obj).IsoCode;

				return this.IsoCode.Equals(isoCode, StringComparison.InvariantCultureIgnoreCase);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Provides a default implementation of GetHashCode.
		/// </summary>
		/// <returns>A hash of the object.</returns>
		public override int GetHashCode()
		{
			return this.IsoCode.GetHashCode();
		}
	}
}
