using System;   //tells compiler where to find Console.WriteLine

namespace DigitalOx //our root name to group our code by
{
    public class Mud2DGame  //our root class for the application
    {
        /// <summary>
        /// This is the main application entry point
        /// </summary>
        /// <param name="args">command line arguments passed into here</param>
        static void Main(string[] args)
        {
            int Player1XPosition = -1;
            int Player1YPosition = -1;

            //read in the map from a txt file
            string[] mapLines = System.IO.File.ReadAllLines("maps/map1.txt");

            //map is 8x8 but lets dynamically determine the size for future use
            int MapYSize = mapLines.Length;
            int MapXSize = mapLines[0].Length;

            //Process map data by looping through each row/string line and then each character in the row
            for (int yPos = 0; yPos < MapYSize; yPos++)
            {
                string currentLine = mapLines[yPos];

                //Safety Check - the line must have some text
                if (string.IsNullOrEmpty(currentLine))
                {
                    continue;
                }

                //CHECK FOR line starting with # = wall, outer edge should all be walls
                if (currentLine[0] != '#')
                {
                    continue;
                }

                //check if player 1 is in this line
                int foundp1 = currentLine.IndexOf('1');
                if(foundp1!=-1) {
                    Player1XPosition = foundp1;
                    Player1YPosition = yPos;
                }
            }

            Console.WriteLine($"Map is loaded...size: [{MapXSize} x {MapYSize}]");
            Console.WriteLine($"Player 1 location: [{Player1XPosition+1}, {Player1YPosition+1}]");

            //draw the map line by line
            for (int line = 0; line < MapYSize; line++)
            {
                Console.WriteLine(mapLines[line]);
            }

        }
    }
}
