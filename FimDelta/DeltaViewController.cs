using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using FimDelta.Xml;
using FimDelta.ViewModel;

namespace FimDelta
{

    public enum GroupType
    {
        None,
        State,
        ObjectType
    }

    public class DeltaViewController
    {
        private readonly Delta delta;
        private GroupType grouping = GroupType.None;
        private IEnumerable view = null;

        public DeltaViewController(Delta delta)
        {
            this.delta = delta;
        }

        public GroupType Grouping
        {
            get { return grouping; }
            set
            {
                grouping = value;
                view = null;
            }
        }

        public IEnumerable View
        {
            get
            {
                if (view == null)
                {
                    switch (Grouping)
                    {
                        case GroupType.None:
                            view = new[] { new GroupByNode("Everything", delta.Objects.Select(t => new ObjectNode(delta, t)).OrderBy(t => t.DisplayName).ToArray()) };
                            break;
                        case GroupType.State:
                            view = delta.Objects.GroupBy(x => x.State).Select(x => new GroupByNode(x.Key.ToString(), x.Select(t => new ObjectNode(delta, t)).OrderBy(t => t.DisplayName).ToArray()));
                            break;
                        case GroupType.ObjectType:
                            view = delta.Objects.GroupBy(x => x.ObjectType).Select(x => new GroupByNode(x.Key, x.Select(t => new ObjectNode(delta, t)).OrderBy(t => t.DisplayName).ToArray()));
                            break;
                    }
                }
                return view;
            }
        }


    }
}
