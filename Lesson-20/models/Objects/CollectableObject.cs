using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class CollectableObject : ObjectBase
    {
        public string Name { get; set; }
        public Dictionary<string, object> Attrib { get; set; }

        public CollectableObject()
        {
            Attrib = new Dictionary<string, object>();
        }
    }
}
