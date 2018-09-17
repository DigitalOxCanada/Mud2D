using Mud2D.models;
using System;

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
            InitalizeMap();

            Console.WriteLine("Press enter to continue");
            Console.ReadKey();

            GameLoop();
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
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X, TheMap.ThePlayer.Y- 1).GetType() == typeof(MapTileSpace))
                            {
                                TheMap.ThePlayer.Y--;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (TheMap.ThePlayer.Y < TheMap.Height - 1)
                        {
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X, TheMap.ThePlayer.Y + 1).GetType() == typeof(MapTileSpace))
                            {
                                TheMap.ThePlayer.Y++;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (TheMap.ThePlayer.X < TheMap.Width - 1)
                        {
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X + 1, TheMap.ThePlayer.Y).GetType() == typeof(MapTileSpace))
                            {
                                TheMap.ThePlayer.X++;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (TheMap.ThePlayer.X > 0)
                        {
                            if (TheMap.GetTileAtPos(TheMap.ThePlayer.X - 1, TheMap.ThePlayer.Y).GetType() == typeof(MapTileSpace))
                            {
                                TheMap.ThePlayer.X--;
                            }
                        }
                        break;
                }
            }
        }


        private static void InitalizeMap()
        {
            string[] mapLines = System.IO.File.ReadAllLines("maps/map1.txt");

            //Create new map instance by dimenions
            TheMap = new Map(mapLines[0].Length, mapLines.Length);

            //FIRST TIME ANALYZE MAP DATA
            for (int yPos = 0; yPos < mapLines.Length; yPos++)
            {
                string currentLine = mapLines[yPos];

                //SAFETY CHECK
                if (string.IsNullOrEmpty(currentLine))
                {
                    continue;
                }

                //create the Map 2d array data from the current line char by char
                for (int xPos = 0; xPos < TheMap.Width; xPos++)
                {
                    TheMap.CreateTile(currentLine[xPos], xPos, yPos);
                }

                //check if player 1 is in this line
                int foundp1 = currentLine.IndexOf('1');
                if(foundp1!=-1) {
                    TheMap.ThePlayer.X = foundp1;
                    TheMap.ThePlayer.Y = yPos;
                }

            }

            Console.WriteLine($"Map is loaded...size: [{TheMap.Width} x {TheMap.Height}]");
            Console.WriteLine($"Player 1 location: [{TheMap.ThePlayer.X+1}, {TheMap.ThePlayer.Y+1}]");
        }
    }
}
