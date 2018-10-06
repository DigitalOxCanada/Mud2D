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
        public ActionObject ActionObject { get; set; }

        public MapTile()
        {
            FOW = (GameEngine.IsFOWEnabled == true ? 2 : 0);
            Dirty = true;
            ActionObject = null;
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
                    if (ActionObject != null)
                    {
                        ActionObject.Draw();
                        
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
