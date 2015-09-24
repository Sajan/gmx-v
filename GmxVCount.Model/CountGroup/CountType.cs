using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
   
    public enum CountType
    {

        [XmlEnum("TotalCharacterCount")]
        TotalCharacterCount,
        [XmlEnum("TotalWordCount")]
        TotalWordCount
    }
}
