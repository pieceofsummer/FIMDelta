using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FimDelta.Xml;

namespace FimDelta.ViewModel
{

    /// <summary>
    /// Represents list of nodes referencing parent object node (for tracking references)
    /// </summary>
    public class ReferencedByNode : INode
    {
        private readonly Delta delta;
        private readonly ImportObject obj;
        private ObjectNode[] children = null;

        public ReferencedByNode(Delta delta, ImportObject obj)
        {
            this.delta = delta;
            this.obj = obj;
        }

        public string DisplayName
        {
            get { return "Referenced by"; }
        }

        public IEnumerable<INode> ChildNodes
        {
            get
            {
                if (delta == null) return null;
                if (obj.State == DeltaState.Delete) return null;

                if (children == null)
                {
                    var id = obj.SourceObjectIdentifier;
                    if (id.StartsWith("urn:uuid:"))
                        id = id.Substring(9);

                    children = delta.Objects
                        .Where(x => x.Changes != null &&
                                    x.Changes.Any(y => y.AttributeValue != null &&
                                                       y.AttributeValue.IndexOf(id, StringComparison.OrdinalIgnoreCase) >= 0))
                        .Select(x => new ObjectNode(delta, x))
                        .ToArray();
                }

                return children;
            }
        }
    }

}
