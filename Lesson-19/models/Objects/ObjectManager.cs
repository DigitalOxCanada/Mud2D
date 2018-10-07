using System;
using System.Linq;

namespace Mud2D.models
{
    public class ObjectManager
    {
        public ObjectManager()
        {

        }

        internal void AddObjectsToMap(int num)
        {
            Random randgen = new Random();

            for (int i = 0; i < num; i++)
            {
                MapTile t = GameEngine.GetRandomTileSpace();

                int r = randgen.Next(0, 10);
                
                switch(r)
                {
                    case 0:
                        t.ActionObject = new ObjectTeleport();
                        break;

                    case 1:
                        t.ActionObject = new ObjectGold();
                        break;

                    case 2:
                        t.ActionObject = new ObjectTrap();
                        break;

                    case 3:
                        t.ActionObject = new ObjectKey();
                        break;

                    case 4:
                        t.ActionObject = new ObjectSword();
                        break;

                    case 5:
                        t.ActionObject = new ObjectTorch();
                        break;

                    default:
                        t.ActionObject = new ObjectGold();
                        break;
                }

                
                //make gold object and assign it to the tile and set the objects X,Y so the object can remove itself later.
                t.ActionObject.X = t.X;
                t.ActionObject.Y = t.Y;
            }
        }

    }
}
