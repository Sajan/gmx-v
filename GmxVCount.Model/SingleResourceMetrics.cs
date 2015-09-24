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
    [Serializable]
    [XmlRoot("metrics", Namespace = "urn:lisa-metrics-tags")]
    public class SingleResourceMetrics : BaseMetrics
    {
        [XmlElement("stage")]
        public List<Stage> Stages { get; set; }

        public SingleResourceMetrics():base()
        {
            Stages = new List<Stage>();
        }
        
        /// <summary>
        /// Gets the Total Word Count of the first Count Group of the first Stage.
        /// Useful for singular word count holding, such to wrap legacy word counting algorithms.
        /// </summary>
        /// <returns></returns>
        public int GetFirstStageWordCount()
        {
            var firstStage = Stages.FirstOrDefault();
            if (firstStage == null)
                throw new ApplicationException("There are no Stages defined in this SingleResourceMetrics");

            var firstCountGroup = firstStage.CountGroups.FirstOrDefault();
            if (firstCountGroup == null)
                throw new ApplicationException("There is no Count Group in the first Stage of this SingleResourceMetrics");

            var totalWordCount = firstCountGroup.Counts.FirstOrDefault(c => c.CountType == CountType.TotalWordCount);
            if (totalWordCount == null)
                throw new ApplicationException("No TotalWordCount defined in the first Count Group of the first Stage in this SingleResourceMetrics");

            return totalWordCount.Value;
        }        
    }
}
