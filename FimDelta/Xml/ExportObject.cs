using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FimDelta.Xml
{

    [Serializable]
    public class ExportObject
    {
        [XmlElement("Source")]
        public string SourceWebService { get; set; }

        [XmlElement("ResourceManagementObject")]
        public ResourceManagementObject Object { get; set; }
    }

}
