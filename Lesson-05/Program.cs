using Mud2D.models;
using System;

namespace DigitalOx
{
    public class Mud2DGame
    {
        static public Map TheMap { get; set; }

        static private int Player1XPosition = 0;
        static private int Player1YPosition = 0;
        static private char[,] Players;

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
                        if (Player1YPosition > 0)
                        {
                            //if the space moving upwards is a blank space then move up
                            if (TheMap.GetTileAtPos(Player1XPosition, Player1YPosition - 1).GetType() == typeof(MapTileSpace))
                            {
                                Player1YPosition--;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Player1YPosition < TheMap.Height - 1)
                        {
                            if (TheMap.GetTileAtPos(Player1XPosition + 1, Player1YPosition + 1).GetType() == typeof(MapTileSpace))
                            {
                                Player1YPosition++;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Player1XPosition < TheMap.Width - 1)
                        {
                            if (TheMap.GetTileAtPos(Player1XPosition + 1, Player1YPosition).GetType() == typeof(MapTileSpace))
                            {
                                Player1XPosition++;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Player1XPosition > 0)
                        {
                            if (TheMap.GetTileAtPos(Player1XPosition - 1, Player1YPosition).GetType() == typeof(MapTileSpace))
                            {
                                Player1XPosition--;
                            }
                        }
                        break;
                }

                //Clear the players 2d array
                //NOTE this won't work well if we have multiple players but this a is simple way for now
                Array.Clear(Players, 0, Players.Length);
                //Place the player 1 in the players 2d array at the new position
                Players[Player1YPosition, Player1XPosition] = '1';
            }
        }


        private static void InitalizeMap()
        {
            string[] mapLines = System.IO.File.ReadAllLines("maps/map1.txt");

            //Create new map instance by dimenions
            TheMap = new Map(mapLines[0].Length, mapLines.Length);

            Players = new char[TheMap.Height, TheMap.Width];

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
                    Player1XPosition = foundp1;
                    Player1YPosition = yPos;
                    Players[yPos, foundp1] = '1';
                }

            }

            Console.WriteLine($"Map is loaded...size: [{TheMap.Width} x {TheMap.Height}]");
            Console.WriteLine($"Player 1 location: [{Player1XPosition+1}, {Player1YPosition+1}]");
        }
    }
}
