using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class XY
    {
        public int x;
        public int y;
    }

    public class ScoreCard
    {
        private string[] scrn = new string[] {
            "**************** Welcome to Mud2D ****************",
            "* Name:                       Gold:              *",
            "*                             Life:              *",
            "**************************************************",
            "*                                                *"
        };

        const int Left = 62;
        static readonly XY namePos = new XY() { x = 9, y = 2 };
        static readonly XY goldPos = new XY() { x = 37, y = 2 };
        static readonly XY lifePos = new XY() { x = 37, y = 3 };
        static readonly XY inventoryPos = new XY() { x = 4, y = 5 };

        public bool Dirty { get; set; }

        public ScoreCard()
        {
            Dirty = true;
        }

        public void Draw()
        {
            int y = 0;
            foreach (var ln in scrn)
            {
                Console.SetCursorPosition(62, y++);
                Console.Write(ln);
            }

            Console.SetCursorPosition(Left + namePos.x, namePos.y - 1);
            Console.Write(GameEngine.ThePlayer.Name.PadRight(20).Substring(0, 20));

            Console.SetCursorPosition(Left + goldPos.x, goldPos.y - 1);
            Console.Write(GameEngine.ThePlayer.Gold.ToString().PadLeft(10, '0'));

            Console.SetCursorPosition(Left + lifePos.x, lifePos.y - 1);
            Console.Write(GameEngine.ThePlayer.Life);

            int cnt = 0;
            foreach (var ln in GameEngine.ThePlayer.Inventory)
            {
                Console.SetCursorPosition(Left + inventoryPos.x, inventoryPos.y + cnt++);
                Console.Write(ln.Name);
                if (ln.Attrib.ContainsKey("Damage"))
                {
                    int dam = Convert.ToInt16(ln.Attrib["Damage"]);
                    Console.Write($" (+{dam})");
                }
            }
        }

        internal void Update()
        {
            if (Dirty)
            {
                Draw();
                Dirty = false;
            }
        }
    }
}
