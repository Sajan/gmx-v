using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{ 
    public class Project : IdentifierXmlAttribute
    {
        [XmlElement("resource")]
        public List<Resource> Resouce { get; set; }
    }
}
