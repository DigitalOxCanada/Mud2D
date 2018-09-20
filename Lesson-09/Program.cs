using Mud2D.models;
using System;
using Figgle;

namespace DigitalOx
{
    public class Mud2DGame
    {
        static public Map TheMap { get; set; }

        /// <summary>
        /// This is the main application entry point
        /// </summary>
        /// <param name="args">command line arguments passed into here</param>
        static void Main(string[] args)
        {
            Intro();

            InitializeMap("maps/map1.txt");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            GameLoop();
        }

        private static void Intro()
        {
            Console.Clear();
            Console.WriteLine(FiggleFonts.Ogre.Render("Mud2D"));
#if DEBUG
            Console.WriteLine("DEBUG version");
#elif RELEASE
            Console.WriteLine("RELEASE version");
#endif
            Console.WriteLine(FiggleFonts.Standard.Render("Lets Go!"));
        }

        /// <summary>
        /// This is the main game loop handles keyboard input
        /// </summary>
        private static void GameLoop()
        {
            bool running = true;

            while (running)
            {
                TheMap.Draw();
                var ch = Console.ReadKey(true).Key;
                switch (ch)
                {
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                    case ConsoleKey.UpArrow:
                        //perform boundary checking
                        if (TheMap.ThePlayer.X > 0)
                        {
                            //if the space moving upwards is a blank space then move up
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X, TheMap.ThePlayer.Y - 1).IsWalkable)
                            {
                                TheMap.MovePlayer(PlayerMovement.Up);
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (TheMap.ThePlayer.Y < TheMap.Height - 1)
                        {
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X, TheMap.ThePlayer.Y + 1).IsWalkable)
                            {
                                TheMap.MovePlayer(PlayerMovement.Down);
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (TheMap.ThePlayer.X < TheMap.Width - 1)
                        {
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X + 1, TheMap.ThePlayer.Y).IsWalkable)
                            {
                                TheMap.MovePlayer(PlayerMovement.Right);
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (TheMap.ThePlayer.X > 0)
                        {
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X - 1, TheMap.ThePlayer.Y).IsWalkable)
                            {
                                TheMap.MovePlayer(PlayerMovement.Left);
                            }
                        }
                        break;
                }
            }
        }


        private static void InitializeMap(string mapFilename)
        {
            TheMap = new Map(mapFilename);

            Console.WriteLine($"Map is loaded...size: [{TheMap.Width} x {TheMap.Height}]");
            Console.WriteLine($"Player 1 location: [{TheMap.ThePlayer.X + 1}, {TheMap.ThePlayer.Y + 1}]");
        }

    }
}
