using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

namespace FimDelta.Xml
{

    [XmlRoot("Results")]
    public class Delta
    {
        [XmlElement("ImportObject")]
        public ImportObject[] Objects { get; set; }

        [XmlIgnore]
        public Export Source { get; set; }

        [XmlIgnore]
        public Export Target { get; set; }

    }
}
