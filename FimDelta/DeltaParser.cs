using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using System.Collections;
using FimDelta.Xml;

namespace FimDelta
{

    class DeltaParser
    {

        public static Delta ReadDelta(string sourceFile, string targetFile, string deltaFile)
        {
            var exportSerializer = new XmlSerializer(typeof(Export));
            var deltaSerializer = new XmlSerializer(typeof(Delta));
            
            Export source, target;
            Delta delta;
            using (var r = XmlReader.Create(sourceFile))
                source = (Export)exportSerializer.Deserialize(r);

            using (var r = XmlReader.Create(targetFile))
                target = (Export)exportSerializer.Deserialize(r);

            using (var r = XmlReader.Create(deltaFile))
                delta = (Delta)deltaSerializer.Deserialize(r);

            delta.Source = source;
            delta.Target = target;

            return delta;
        }

        public static void SaveDelta(Delta delta, string file)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");

            var serializer = new XmlSerializer(typeof(Delta));
            
            var list = new List<ImportObject>();
            foreach (var obj in delta.Objects.Where(x => x.NeedsInclude()))
            {
                var newObj = new ImportObject();
                newObj.SourceObjectIdentifier = obj.SourceObjectIdentifier;
                newObj.TargetObjectIdentifier = obj.TargetObjectIdentifier;
                newObj.ObjectType = obj.ObjectType;
                newObj.State = obj.State;
                newObj.Changes = obj.Changes != null ? obj.Changes.Where(x => x.IsIncluded).ToArray() : null;
                newObj.AnchorPairs = obj.AnchorPairs;
                list.Add(newObj);
            }

            var newDelta = new Delta();
            newDelta.Objects = list.ToArray();

			var settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = false;
			settings.Indent = true;
            using (var w = XmlWriter.Create(file, settings))
                serializer.Serialize(w, newDelta, ns);
        }

    }
}
