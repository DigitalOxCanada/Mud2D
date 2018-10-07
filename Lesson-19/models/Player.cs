using System;
using System.Collections.Generic;

namespace Mud2D.models
{
    enum PlayerMovement
    {
        Up = 1,
        Down,
        Left,
        Right
    }

    public class Player : ObjectBase
    {
        public bool Dirty { get; set; }
        public string Name { get; set; }
        public int MaxLife { get; set; }
        public int Life { get; set; }
        public long Gold { get; set; }
        public List<CollectableObject> Inventory { get; set; }

        public Player()
        {
            Dirty = true;
            Symbol = '1';
            Name = "John Doe";
            Gold = 100;
            Life = 10;
            MaxLife = Life;
            Inventory = new List<CollectableObject>();
            Inventory.Add(new CollectableObject() { Name = "Dagger" });
        }

        public void Update()
        {
            //only erase where it has been if it was a valid space
            if (X < 0 || Y < 0)
            {
                return;
            }

            if (Dirty)
            {
                if (LastX > -1 && LastY > -1)
                {
                    Console.SetCursorPosition(LastX, LastY);
                    GameEngine.Tiles[LastY, LastX].Draw(true);  //redraw the tile we moved off of, in case its stairs or an object we didn't pick up.
                }

                Console.SetCursorPosition(X, Y);
                Console.Write(Symbol);
                Console.SetCursorPosition(0, 0);    //move the cursor away from player so you don't see blinking cursor
                Dirty = false;
            }
        }

    }
}
