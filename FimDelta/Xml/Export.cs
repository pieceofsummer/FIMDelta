using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FimDelta.Xml
{

    [XmlRoot("Results")]
    public class Export
    {
        [XmlElement("ExportObject")]
        public ExportObject[] Objects { get; set; }
    }

}
