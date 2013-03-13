using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FimDelta.Xml
{
    /// <summary>
    /// Operation state for objects
    /// </summary>
    public enum DeltaState
    {
        Create,
        Put,
        Resolve,
        Delete
    }

}
