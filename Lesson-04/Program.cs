using System;

namespace DigitalOx
{
    public class MapSettings
    {
        public int Width;
        public int Height;
        public string Name;
        public int MaxPlayers;
        public string Author;
    }

    public class Mud2DGame
    {
        static private MapSettings mapsettings = new MapSettings();

        static private int Player1XPosition = 0;
        static private int Player1YPosition = 0;
        static private char[,] Map;
        static private char[,] Players;

        /// <summary>
        /// This is the main application entry point
        /// </summary>
        /// <param name="args">command line arguments passed into here</param>
        static void Main(string[] args)
        {
            InitalizeMap();

            GameLoop();
        }

        /// <summary>
        /// This is the main game loop handles keyboard input
        /// </summary>
        private static void GameLoop()
        {
            bool running = true;

            while (running == true)
            {
                DrawMap();
                var ch = Console.ReadKey(false).Key;
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
                            if (Map[Player1YPosition - 1, Player1XPosition] == ' ')
                            {
                                Player1YPosition--;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Player1YPosition < mapsettings.Height - 1)
                        {
                            if (Map[Player1YPosition + 1, Player1XPosition + 1] == ' ')
                            {
                                Player1YPosition++;
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Player1XPosition < mapsettings.Width - 1)
                        {
                            if (Map[Player1YPosition, Player1XPosition + 1] == ' ')
                            {
                                Player1XPosition++;
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Player1XPosition > 0)
                        {
                            if (Map[Player1YPosition, Player1XPosition - 1] == ' ')
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

        private static void DrawMap()
        {
            Console.Clear();
            for (int y = 0; y < mapsettings.Height; y++)
            {
                for (int x = 0; x < mapsettings.Width; x++)
                {
                    if (x == Player1XPosition && y == Player1YPosition)
                    {
                        Console.Write('1');
                    }
                    else
                    {
                        Console.Write(Map[y, x]);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Player 1 X Position = {Player1XPosition + 1}");
            Console.WriteLine($"Player 1 Y Position = {Player1YPosition + 1}");

        }

        private static void InitalizeMap()
        {
            string[] mapLines = System.IO.File.ReadAllLines("maps/map1.txt");
            mapsettings.Height = mapLines.Length;
            mapsettings.Width = mapLines[0].Length;
            Map = new char[mapsettings.Height, mapsettings.Width];
            Players = new char[mapsettings.Height, mapsettings.Width];

            //FIRST TIME ANALYZE MAP DATA
            for (int yPos = 0; yPos < mapLines.Length; yPos++)
            {
                string currentLine = mapLines[yPos];

                //SAFETY CHECK
                if (string.IsNullOrEmpty(currentLine))
                {
                    continue;
                }

                //CHECK FOR line starting with # = wall
                if (currentLine[0] != '#')
                {
                    continue;
                }

                //create the Map 2d array data from the current line char by char
                for (int xPos = 0; xPos < mapsettings.Width; xPos++)
                {
                    Map[yPos, xPos] = currentLine[xPos];
                }

                //check if player 1 is in this line
                int foundp1 = currentLine.IndexOf('1');
                if(foundp1!=-1) {
                    Player1XPosition = foundp1;
                    Player1YPosition = yPos;

                    //clear out the player 1 from the map so the map no longer holds the player data
                    //player data will be on the player array from now on
                    Map[Player1YPosition, Player1XPosition] = ' ';
                }

            }

            Console.WriteLine($"Map is loaded...size: [{mapsettings.Width} x {mapsettings.Height}]");
            Console.WriteLine($"Player 1 location: [{Player1XPosition+1}, {Player1YPosition+1}]");
        }
    }
}
