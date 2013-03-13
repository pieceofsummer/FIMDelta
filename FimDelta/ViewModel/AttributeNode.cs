using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FimDelta.Xml;
using System.ComponentModel;

namespace FimDelta.ViewModel
{

    /// <summary>
    /// Represents an single attribute of an object
    /// </summary>
    public class AttributeNode : IIncludableNode, INotifyPropertyChanged, IDisposable
    {
        private readonly Delta delta;
        private readonly ImportObject obj;
        private readonly ImportChange attr;
        private ObjectNode[] children = null;
        private WeakReference parent = null;

        public AttributeNode(Delta delta, ImportObject obj, ImportChange attr)
        {
            this.delta = delta;
            this.obj = obj;
            this.attr = attr;

            this.attr.PropertyChanged += SourcePropertyChanged;
        }

        ~AttributeNode()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            this.attr.PropertyChanged -= SourcePropertyChanged;
            if (disposing)
                GC.SuppressFinalize(this);
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsIncluded")
                UpdateInclude();
        }

        public ObjectNode Parent
        {
            get { return parent != null && parent.IsAlive ? (ObjectNode)parent.Target : null; }
            set { parent = new WeakReference(value); }
        }

        public string OperationName
        {
            get
            {
                if (attr.Operation == "None")
                    return "Set";
                else
                    return attr.Operation;
            }
        }

        public string AttributeName
        {
            get { return attr.AttributeName; }
        }

        public string AttributeValue
        {
            get { return attr.AttributeValue; }
        }

        public string DisplayTooltip
        {
            get
            {
                return DisplayValue == attr.AttributeValue ? null : attr.AttributeValue;
            }
        }

        public string DisplayValue
        {
            get
            {
                if (string.IsNullOrEmpty(attr.AttributeValue))
                {
                    if (attr.Operation == "None" || attr.Operation == "Set")
                        return "(empty)";
                    else
                        return attr.AttributeValue;
                }

                string s = attr.AttributeValue.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\r", "");

                if (s.Length > 150)
                {
                    s = s.Substring(0, 146) + " ...";
                }

                return s;
            }
        }

        public IEnumerable<INode> ChildNodes
        {
            get 
            {
                if (string.IsNullOrEmpty(attr.AttributeValue)) return null;

                if (children == null)
                {
                    children = delta.Objects
                        .Where(x => (x.SourceObjectIdentifier != null &&
                                     x.SourceObjectIdentifier.StartsWith("urn:uuid:") &&
                                     attr.AttributeValue.IndexOf(x.SourceObjectIdentifier.Substring(9), StringComparison.OrdinalIgnoreCase) >= 0) ||
                                    (x.TargetObjectIdentifier != null &&
                                     x.TargetObjectIdentifier.StartsWith("urn:uuid:") &&
                                     attr.AttributeValue.IndexOf(x.TargetObjectIdentifier.Substring(9), StringComparison.OrdinalIgnoreCase) >= 0))
                        .Select(x => new ObjectNode(delta, x))
                        .ToArray();
                }

                return children;
            }
        }

        private bool inIncludeLoop = false;

        public bool? Include
        {
            get
            {
                if (!obj.IsIncluded)
                    return false;
                return attr.IsIncluded;
            }
            set
            {
                inIncludeLoop = true;

                try
                {
                    attr.IsIncluded = value.GetValueOrDefault();
                    if (attr.IsIncluded)
                        obj.IsIncluded = true;

                    if (children != null)
                        foreach (var ch in children)
                            ch.UpdateInclude();
                }
                finally
                {
                    inIncludeLoop = false;
                }

                UpdateInclude();
            }
        }

        public void UpdateInclude()
        {
            if (inIncludeLoop) return;

            OnPropertyChanged("Include");

            if (Parent != null)
                Parent.UpdateInclude();
        }

        protected void OnPropertyChanged(string property)
        {
            var e = PropertyChanged;
            if (e != null)
                e(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
