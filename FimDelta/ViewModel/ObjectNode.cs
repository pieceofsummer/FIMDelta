using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FimDelta.Xml;

namespace FimDelta.ViewModel
{
    /// <summary>
    /// Represents a single changed object in object tree
    /// </summary>
    public class ObjectNode : IIncludableNode, INotifyPropertyChanged, IDisposable
    {
        private readonly Delta delta;
        private readonly ImportObject obj;
        private readonly AttributeNode[] children;
        private WeakReference parent = null;
        private string displayName = null;

        public ObjectNode(Delta delta, ImportObject obj)
        {
            this.delta = delta;
            this.obj = obj;

            this.obj.PropertyChanged += new PropertyChangedEventHandler(SourcePropertyChanged);

            this.children = null;
            if (obj.Changes != null)
                this.children = obj.Changes.Select(a => new AttributeNode(delta, obj, a) { Parent = this }).ToArray();
        }

        ~ObjectNode()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            this.obj.PropertyChanged -= SourcePropertyChanged;
            if (disposing)
                GC.SuppressFinalize(this);
        }

        private void SourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsIncluded")
                UpdateInclude();
        }

        public IIncludableNode Parent 
        {
            get { return parent != null && parent.IsAlive ? (IIncludableNode)parent.Target : null; }
            set { parent = new WeakReference(value); } 
        }

        public string State
        {
            get { return obj.State.ToString(); }
        }

        public string ObjectType
        {
            get { return obj.ObjectType; }
        }

        public string DisplayName
        {
            get
            {
                if (displayName != null)
                    return displayName;

                if (obj.Changes != null)
                {
                    var c = obj.Changes.FirstOrDefault(x => x.AttributeName == "DisplayName");
                    if (c != null)
                        return displayName = c.AttributeValue;
                }

                if ((obj.State == DeltaState.Delete || obj.State == DeltaState.Put) && delta.Target != null)
                {
                    var nameAttr = delta.Target.Objects
                        .First(x => x.Object.ObjectType == obj.ObjectType && x.Object.ObjectIdentifier == obj.TargetObjectIdentifier)
                        .Object.Attributes.FirstOrDefault(x => x.AttributeName == "DisplayName");
                    if (nameAttr != null)
                        return displayName = nameAttr.Value;
                }

                if (obj.State == DeltaState.Resolve && delta.Source != null)
                {
                    var nameAttr = delta.Source.Objects
                        .First(x => x.Object.ObjectType == obj.ObjectType && x.Object.ObjectIdentifier == obj.SourceObjectIdentifier)
                        .Object.Attributes.FirstOrDefault(x => x.AttributeName == "DisplayName");
                    if (nameAttr != null)
                        return displayName = nameAttr.Value;
                }

                return displayName = "";
            }
        }

        public IEnumerable<INode> ChildNodes
        {
            get
            {
                var list = new List<INode>();

                var refdBy = new ReferencedByNode(delta, obj);
                if (refdBy.ChildNodes != null && refdBy.ChildNodes.Any())
                    list.Add(refdBy);

                if (children != null)
                    list.AddRange(children);

                return list;
            }
        }

        private bool inIncludeLoop = false;

        public bool? Include
        {
            get
            {
                if (!obj.IsIncluded) return false;

                if (obj.Changes != null)
                {
                    bool hasIncluded = false, hasExcluded = false;
                    foreach (var ch in obj.Changes)
                    {
                        if (ch.IsIncluded)
                            hasIncluded = true;
                        else
                            hasExcluded = true;

                        if (hasIncluded && hasExcluded) return null;
                    }

                    return hasIncluded;
                }

                return obj.IsIncluded;
            }
            set
            {
                inIncludeLoop = true;

                try
                {
                    obj.IsIncluded = value.GetValueOrDefault();
                    if (obj.Changes != null)
                        foreach (var ch in obj.Changes)
                            ch.IsIncluded = obj.IsIncluded;

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

            var p = Parent;
            if (p != null)
                p.UpdateInclude();
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
