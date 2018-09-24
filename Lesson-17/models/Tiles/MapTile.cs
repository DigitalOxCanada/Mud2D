using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class MapTile : ObjectBase
    {
        public bool IsWalkable { get; set; }
        public int FOW { get; set; }
        public bool Dirty { get; internal set; }
        public ActionObject actionObject { get; set; }


        public MapTile()
        {
#if FOW
            FOW = 2;
#endif
            Dirty = true;
            actionObject = null;
        }

        internal void Draw(bool force = false)
        {
            if (Dirty || force)
            {
                if (FOW > 0)
                {
                    Console.SetCursorPosition(X, Y);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write('█'); //fog
                }
                else
                {
                    Console.SetCursorPosition(X, Y);
                    Console.ForegroundColor = ConsoleColor.White;
                    if (actionObject != null)
                    {
                        actionObject.Draw();
                        
                    }
                    else
                    {
                        Console.Write(Symbol);
                    }
                }
                Dirty = false;
            }
        }

        internal void ClearFOG()
        {
            if(FOW>0)
            {
                FOW = 0;
                Dirty = true;
            }
        }
    }
}
