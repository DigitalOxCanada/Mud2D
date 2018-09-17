<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

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

        public Player ThePlayer { get; set; }   //NOTE single for now, eventually we use an array or list of players
        public List<Monster> Monsters { get; set; }


        
        /// <summary>
        /// Constructor creates the map tiles array by dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(string mapFilename)
        {
            string[] mapLines = System.IO.File.ReadAllLines(mapFilename);
            
            MaxPlayers = 1;
            Width = mapLines[0].Length;
            Height = mapLines.Length;
            Tiles = new MapTile[Height, Width];
            ThePlayer = new Player();
            Monsters = new List<Monster>();

            for (int yPos = 0; yPos < Height; yPos++)
            {
                string currentLine = mapLines[yPos];

                //SAFETY CHECK
                if (string.IsNullOrEmpty(currentLine))
                {
                    continue;
                }

                //create the Map 2d array data from the current line char by char
                for (int xPos = 0; xPos < Width; xPos++)
                {
                    CreateTile(currentLine[xPos], xPos, yPos);
                }

                //check if player 1 is in this line
                int foundp1 = currentLine.IndexOf('1');
                if (foundp1 != -1)
                {
                    ThePlayer.X = foundp1;
                    ThePlayer.Y = yPos;
                }

            }
            
            var blanks = (from t in Tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) select t).ToArray();
            Random randgen = new Random();
            var blank = blanks[randgen.Next(blanks.Length)];

            Monsters.Add(new Monster() { Name = "Ogre", Life = 5, X=blank.X, Y=blank.Y });
            Tiles[blank.Y, blank.X].IsWalkable = false;
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
                //blank tile with a player on it
                case '1':
                    {
                        MapTileSpace t = new MapTileSpace();
                        t.X = x;
                        t.Y = y;
                        Tiles[y, x] = t;

                        ThePlayer.X = x;
                        ThePlayer.Y = y;
                    }
                    break;
            }
            return Tiles[y,x];
        }

        internal void MovePlayer(PlayerMovement dir)
        {
            Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = true;
            switch (dir)
            {
                case PlayerMovement.Left:
                    ThePlayer.X--;
                    break;
                case PlayerMovement.Right:
                    ThePlayer.X++;
                    break;
                case PlayerMovement.Up:
                    ThePlayer.Y--;
                    break;
                case PlayerMovement.Down:
                    ThePlayer.Y++;
                    break;
            }
            Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = false;
        }

        /// <summary>
        /// Draw the map
        /// </summary>
        public void Draw()
        {
            Console.Clear();
            var origRow = Console.CursorTop;
            var origCol = Console.CursorLeft;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tiles[y, x].Draw();
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Player {ThePlayer.Number} location: [{ThePlayer.X + 1}, {ThePlayer.Y + 1}]");

            ThePlayer.Draw();

            foreach(var m in Monsters)
            {
                m.Draw();
            }

            Console.SetCursorPosition(0, 0);
            foreach (var m in Monsters)
            {
                m.PerformAI(this);
            }
        }

        internal MapTile GetTileAtPos(int x, int y)
        {
            return Tiles[y, x];
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

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

        public Player ThePlayer { get; set; }   //NOTE single for now, eventually we use an array or list of players
        public List<Monster> Monsters { get; set; }


        
        /// <summary>
        /// Constructor creates the map tiles array by dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(string mapFilename)
        {
            string[] mapLines = System.IO.File.ReadAllLines(mapFilename);
            
            MaxPlayers = 1;
            Width = mapLines[0].Length;
            Height = mapLines.Length;
            Tiles = new MapTile[Height, Width];
            ThePlayer = new Player();
            Monsters = new List<Monster>();

            for (int yPos = 0; yPos < Height; yPos++)
            {
                string currentLine = mapLines[yPos];

                //SAFETY CHECK
                if (string.IsNullOrEmpty(currentLine))
                {
                    continue;
                }

                //create the Map 2d array data from the current line char by char
                for (int xPos = 0; xPos < Width; xPos++)
                {
                    CreateTile(currentLine[xPos], xPos, yPos);
                }

                //check if player 1 is in this line
                int foundp1 = currentLine.IndexOf('1');
                if (foundp1 != -1)
                {
                    ThePlayer.X = foundp1;
                    ThePlayer.Y = yPos;
                }

            }
            
            var blanks = (from t in Tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) select t).ToArray();
            Random randgen = new Random();
            var blank = blanks[randgen.Next(blanks.Length)];

            Monsters.Add(new Monster() { Name = "Ogre", Life = 5, X=blank.X, Y=blank.Y });
            Tiles[blank.Y, blank.X].IsWalkable = false;
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
                //blank tile with a player on it
                case '1':
                    {
                        MapTileSpace t = new MapTileSpace();
                        t.X = x;
                        t.Y = y;
                        Tiles[y, x] = t;

                        ThePlayer.X = x;
                        ThePlayer.Y = y;
                    }
                    break;
            }
            return Tiles[y,x];
        }

        internal void MovePlayer(PlayerMovement dir)
        {
            Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = true;
            switch (dir)
            {
                case PlayerMovement.Left:
                    ThePlayer.X--;
                    break;
                case PlayerMovement.Right:
                    ThePlayer.X++;
                    break;
                case PlayerMovement.Up:
                    ThePlayer.Y--;
                    break;
                case PlayerMovement.Down:
                    ThePlayer.Y++;
                    break;
            }
            Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = false;
        }

        /// <summary>
        /// Draw the map
        /// </summary>
        public void Draw()
        {
            Console.Clear();
            var origRow = Console.CursorTop;
            var origCol = Console.CursorLeft;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tiles[y, x].Draw();
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Player {ThePlayer.Number} location: [{ThePlayer.X + 1}, {ThePlayer.Y + 1}]");

            ThePlayer.Draw();

            foreach(var m in Monsters)
            {
                m.Draw();
            }

            Console.SetCursorPosition(0, 0);
            foreach (var m in Monsters)
            {
                m.PerformAI(this);
            }
        }

        internal MapTile GetTileAtPos(int x, int y)
        {
            return Tiles[y, x];
        }
    }
}
>>>>>>> 4c4388af791c5718fe4a26b8dff267c865bff09c
