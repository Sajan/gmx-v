using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GmxVCount.Model
{
    public class PhaseType
    {
        [XmlText]
        public string ThePhaseType { get; set; }

        private PhaseType(string phaseType)
        {
            ThePhaseType = phaseType;
        }

        public static PhaseType FromPhaseType(string phaseType)
        {
            return new PhaseType(phaseType);
        }

        public static PhaseType Initial
        {
            get { return _initial; }
        }

        private static readonly PhaseType _initial = FromPhaseType("initial");


        public static PhaseType Final
        {
            get { return _final; }
        }

        private static readonly PhaseType _final = FromPhaseType("final");


        public override string ToString()
        {
            return ThePhaseType;
        }
    }
}
