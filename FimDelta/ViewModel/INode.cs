using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace FimDelta.ViewModel
{
    public interface INode
    {
        IEnumerable<INode> ChildNodes { get; }
    }

    public interface IIncludableNode : INode
    {
        bool? Include { get; set; }
        void UpdateInclude();
    }
}
