using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    [DataContract]
    public enum VerifiableType
    {
        [XmlEnum("non-verifiable")]
        NonVerifiable,
        [XmlEnum("verifiable")]
        Verifiable
    }
}
