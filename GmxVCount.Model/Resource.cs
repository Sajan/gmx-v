using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    public class Resource : IdentifierXmlAttribute
    { 
        [XmlElement("stage")]
        public List<Stage> Stages { get; set; }
    }
}
