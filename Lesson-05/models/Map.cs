using System;
using System.Collections.Generic;
using System.Text;

namespace Mud2D.models
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }
        public string Author { get; set; }

        public MapTile[,] Tiles { get; set; }


        /// <summary>
        /// Constructor creates the map tiles array by dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new MapTile[height, width];
        }

        /// <summary>
        /// Pass a text symbol and the appropriate type gets created and added to the map
        /// </summary>
        /// <param name="symbol"></param>
        public MapTile CreateTile(char symbol, int x, int y)
        {
            switch(symbol)
            {
                //blank tile
                case ' ':
                    {
                        MapTileSpace t = new MapTileSpace();
                        t.X = x;
                        t.Y = y;
                        Tiles[y, x] = t;
                    }
                    break;
                //wall tile
                case '#':
                    {
                        MapTileWall w = new MapTileWall();
                        w.X = x;
                        w.Y = y;
                        Tiles[y, x] = w;
                    }
                    break;
                //door tile
                case '%':
                    {
                        MapTileDoor d = new MapTileDoor();
                        d.X = x;
                        d.Y = y;
                        Tiles[y, x] = d;
                    }
                    break;
                //blank tile
                case '1':
                    {
                        MapTileSpace t = new MapTileSpace();
                        t.X = x;
                        t.Y = y;
                        Tiles[y, x] = t;
                        //TODO add code to create player object
                    }
                    break;
            }
            return Tiles[y,x];
        }

        /// <summary>
        /// Draw the map
        /// </summary>
        public void Draw()
        {
            Console.Clear();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    //if (x == Player1XPosition && y == Player1YPosition)
                    //{
                    //    Console.Write('1');
                    //}
                    //else
                    //{
                    //    Console.Write(Map[y, x]);
                    Console.Write(Tiles[y, x].Symbol);
                    //}
                }
                Console.WriteLine();
            }
//            Console.WriteLine($"Player 1 location: [{Player1XPosition + 1}, {Player1YPosition + 1}]");
        }

        internal MapTile GetTileAtPos(int x, int y)
        {
            return Tiles[y, x];
        }
    }
}
