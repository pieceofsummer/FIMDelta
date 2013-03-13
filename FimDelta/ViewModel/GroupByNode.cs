using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FimDelta.Xml;
using System.Collections;

namespace FimDelta.ViewModel
{

    /// <summary>
    /// Represents a group node, with ability to check/uncheck all the children
    /// </summary>
    public class GroupByNode : IIncludableNode, INotifyPropertyChanged
    {
        private readonly string name;
        private readonly IEnumerable<ObjectNode> children;

        public GroupByNode(string name, IEnumerable<ObjectNode> children)
        {
            this.name = name;
            this.children = children;

            foreach (var child in children)
                child.Parent = this;
        }

        public string Name { get { return name; } }

        public IEnumerable<INode> ChildNodes { get { return children.Cast<INode>(); } }

        private bool inIncludeLoop = false;

        public bool? Include
        {
            get
            {
                if (children.All(x => x.Include != false)) return true;
                if (children.All(x => x.Include == false)) return false;
                return null;
            }
            set
            {
                inIncludeLoop = true;
                try
                {
                    foreach (var child in children)
                        child.Include = value.GetValueOrDefault();
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
