using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    public class Stage
    {
        [XmlAttribute("phase")]
        public string PhaseTypeString
        {
            get { return Phase.ToString(); }
            set { Phase = PhaseType.FromPhaseType(value);}
        } 

        [XmlIgnore]
        public PhaseType Phase { get; set; }

        [XmlAttribute("date")]
        public DateTime Date { get; set; }

        [XmlAttribute("source-language")]
        public string SourceLanguage { get; set; }

        [XmlAttribute("target-language")]
        public string TargetLanguage { get; set; }

        [XmlElement("count-group")]
        public List<CountGroup> CountGroups { get; set; }

        public Stage()
        {
            CountGroups = new List<CountGroup>();
        }


    }
}
