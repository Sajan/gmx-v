using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    [Serializable]
    [DataContract]
    [XmlRoot("metrics", Namespace = "urn:lisa-metrics-tags")]
    public class ProjectMetrics : BaseMetrics
    {
        [XmlElement("project")]
        public List<Project> Projects { get; set; }

        public bool ShouldSerializeProject()
        {
            return Projects != null && Projects.Any();
        }
    }
}
