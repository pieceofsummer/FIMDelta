using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FimDelta.Xml
{

    [Serializable]
    public class JoinPair
    {
        [XmlElement]
        public string AttributeName { get; set; }

        [XmlElement]
        public string AttributeValue { get; set; }
    }

}
