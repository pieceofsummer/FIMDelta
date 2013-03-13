using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FimDelta.Xml
{

    [Serializable]
    public class ResourceManagementAttribute
    {
        [XmlElement]
        public string AttributeName { get; set; }

        [XmlElement]
        public bool HasReference { get; set; }

        [XmlElement]
        public bool IsMultiValue { get; set; }

        [XmlElement]
        public string Value { get; set; }

        [XmlArray("Values"), XmlArrayItem("string")]
        public string[] Values { get; set; }
    }

}
