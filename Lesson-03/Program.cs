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
        //Use a settings class to store the info about the map
        static private MapSettings mapsettings = new MapSettings();

        static private int Player1XPosition = 0;
        static private int Player1YPosition = 0;
        static private char[,] Map;

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
                //Draw the map so the user knows where they are
                DrawMap();
                //take input from the user
                var ch = Console.ReadKey(false).Key;
                switch (ch)
                {
                    //Escape quits the endless while loop and the program ends
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                    //move the player 1 position up,down,right,left
                    //the problem at this point is no boundary checking so the user can 
                    //literally go off the map causing an exception
                    case ConsoleKey.UpArrow:
                        Player1YPosition--;
                        break;
                    case ConsoleKey.DownArrow:
                        Player1YPosition++;
                        break;
                    case ConsoleKey.RightArrow:
                        Player1XPosition++;
                        break;
                    case ConsoleKey.LeftArrow:
                        Player1XPosition--;
                        break;
                }
            }
        }

        /// <summary>
        /// Draw the map char by char and the player 1
        /// </summary>
        private static void DrawMap()
        {
            for (int y = 0; y < mapsettings.Height; y++)
            {
                for (int x = 0; x < mapsettings.Width; x++)
                {
                    //if this position is where player 1 is then print 1
                    if (x == Player1XPosition && y == Player1YPosition)
                    {
                        Console.Write('1');
                    }
                    else
                    {
                        //draw the single cell of the map
                        Console.Write(Map[y, x]);
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Player 1 location: [{Player1XPosition + 1},{Player1YPosition + 1}]");
        }

        /// <summary>
        /// Read and parse the map
        /// </summary>
        private static void InitalizeMap()
        {
            string[] mapslines = System.IO.File.ReadAllLines("maps/map1.txt");
            mapsettings.Height = mapslines.Length;
            mapsettings.Width = mapslines[0].Length;
            Map = new char[mapsettings.Height, mapsettings.Width];

            //FIRST TIME ANALYZE MAP DATA
            for (int yPos = 0; yPos < mapslines.Length; yPos++)
            {
                string currentLine = mapslines[yPos];

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
