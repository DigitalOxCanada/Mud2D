<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileDoor : MapTile
    {
        public bool IsLocked { get; set; }

        public MapTileDoor()
        {
            Symbol = '%';
            IsLocked = false;
            IsWalkable = false;
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTileDoor : MapTile
    {
        public bool IsLocked { get; set; }

        public MapTileDoor()
        {
            Symbol = '%';
            IsLocked = false;
            IsWalkable = false;
        }
    }
}
>>>>>>> 4c4388af791c5718fe4a26b8dff267c865bff09c
