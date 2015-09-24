using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    public class Count
    {
    
        [XmlAttribute("type")]
        public CountType CountType { get; set; }

        [XmlAttribute("value")]
        public int Value { get; set; }
    }
}
