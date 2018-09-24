using System;
using System.Linq;

namespace Mud2D.models
{
    public class ObjectManager
    {
        public ObjectManager()
        {

        }

        internal void AddObjects(int num)
        {
            Random randgen = new Random();

            for (int i = 0; i < num; i++)
            {
                MapTile t = GameEngine.GetRandomTileSpace();

                int r = randgen.Next(0, 2);
                
                switch(r)
                {
                    case 0:
                        t.actionObject = new ObjectTeleport();
                        break;

                    case 1:
                        t.actionObject = new ObjectGold();
                        break;

                    case 2:
                        t.actionObject = new ObjectTrap();
                        break;

                    default:
                        t.actionObject = new ObjectGold();
                        break;
                }

                
                //make gold object and assign it to the tile and set the objects X,Y so the object can remove itself later.
                t.actionObject.X = t.X;
                t.actionObject.Y = t.Y;
            }
        }

    }
}
