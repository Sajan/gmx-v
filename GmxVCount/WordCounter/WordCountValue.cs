using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GmxVCount.TextBlocks;
using GmxVCount.Model;

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
       /// 
       /// </summary>
       /// <param name="sourceCulture"></param>
       /// <param name="totalWordCount"></param>
       /// <param name="totalCharacterCount"></param>
       /// <returns></returns>
		public static SingleResourceMetrics ToGmxV(Language sourceCulture, int totalWordCount, int totalCharacterCount)
       {
          return new SingleResourceMetrics()
           {
               ToolName = "Gould Tech Solutions GMX/V Word Counter",
               ToolVersion = "1.0",
               Stages =
               {
                   new Stage()
                   {
                       Phase = PhaseType.Initial,
                       SourceLanguage = sourceCulture.IsoCode,
                       Date = DateTime.Now,
                       CountGroups =
                       {
                           new CountGroup()
                           {
                               Name = VerifiableType.Verifiable,
                               Counts =
                               {
                                   new Count()
                                   {
                                       CountType = CountType.TotalCharacterCount,
                                       Value = totalCharacterCount
                                   },
                                   new Count()
                                   {
                                       CountType = CountType.TotalWordCount,
                                       Value = sourceCulture.IsLogographicLanguage?LogographicWordCounts(sourceCulture, totalCharacterCount):totalWordCount
                                   }
                               }
                           }
                       }
                   }
               }
           };
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCulture"></param>
        /// <param name="totalCharacterCount"></param>
        /// <returns></returns>
	    private static int LogographicWordCounts(Language sourceCulture, int totalCharacterCount)
	    {
	        switch (sourceCulture.Root.IsoCode)
	        {
	            case "zh":
	                return Convert.ToInt32(totalCharacterCount/2.8);
                case "ja":
	                return totalCharacterCount/3;
	            case "ko":
	                return Convert.ToInt32(totalCharacterCount/3.3);
	            case "th":
                case "lo":
                case "km":
                case "my":
	                return totalCharacterCount/6;
                default:
	                return 0;
	        }
	    }

	    /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
	    public SingleResourceMetrics ToGmxV()
	    {
	        return ToGmxV(this.SourceCulture,this.WordCount,this.CharacterCount);
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
