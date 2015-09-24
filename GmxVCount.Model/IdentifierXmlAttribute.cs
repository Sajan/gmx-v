using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    public class IdentifierXmlAttribute
    {
        [XmlAttribute("identifier")]
        public string Identifier { get; set; }
    }
}
