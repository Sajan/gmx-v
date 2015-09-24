using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    public class CountGroup
    {
     

        [XmlAttribute("name")]
        public VerifiableType Name { get; set; }

        [XmlElement("count")]
        public List<Count> Counts { get; set; }

        public CountGroup()
        {
            Counts = new List<Count>();
        }
    }
}
