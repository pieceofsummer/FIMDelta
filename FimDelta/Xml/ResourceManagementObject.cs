using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FimDelta.Xml
{

    [Serializable]
    public class ResourceManagementObject
    {
        [XmlElement]
        public string ObjectIdentifier { get; set; }

        [XmlElement]
        public string ObjectType { get; set; }

        [XmlElement]
        public bool IsPlaceholder { get; set; }

        [XmlArray("ResourceManagementAttributes"), XmlArrayItem("ResourceManagementAttribute")]
        public ResourceManagementAttribute[] Attributes { get; set; }
    }

}
