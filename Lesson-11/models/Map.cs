using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using Serilog;

namespace Mud2D.models
{
    public class Map
    {
        private bool NeedsRedrawing { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }
        public string Author { get; set; }

        public MapTile[,] Tiles { get; set; }

        public Player ThePlayer { get; set; }   //NOTE single for now, eventually we use an array or list of players
        public MonsterManager MonsterMgr { get; set; }


        
        /// <summary>
        /// Constructor creates the map tiles array by dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Map(string mapFilename)
        {
            string[] mapLines = System.IO.File.ReadAllLines(mapFilename);

            NeedsRedrawing = true;  //when loading a new map we trigger redrawing map
            MaxPlayers = 1;
            Width = mapLines[0].Length;
            Height = mapLines.Length;
            Tiles = new MapTile[Height, Width];
            ThePlayer = new Player();
            MonsterMgr = new MonsterManager();

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

            MonsterMgr.AddMonster(Tiles, 3);
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
                //stairs up tile
                case '«':
                    {
                        MapTileStairs d = new MapTileStairs(1);
                        d.X = x;
                        d.Y = y;
                        Tiles[y, x] = d;
                    }
                    break;
                //stairs down tile
                case '»':
                    {
                        MapTileStairs d = new MapTileStairs(-1);
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
            ThePlayer.NeedsRedrawing = true;
            Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = true;
            ThePlayer.LastX = ThePlayer.X;
            ThePlayer.LastY = ThePlayer.Y;
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
        public int Update()
        {
            var origRow = Console.CursorTop;
            var origCol = Console.CursorLeft;

            if (NeedsRedrawing)
            {
                Console.Clear();

                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Tiles[y, x].Draw();
                    }
                    Console.WriteLine();    //carriage return at end of each line because we're not using SetCursorPosition.
                }
                NeedsRedrawing = false;
            }

            //if there is a keypress available to capture, take it and perform its action
            if (Console.KeyAvailable)
            {
                var ch = Console.ReadKey(true).Key;
                switch (ch)
                {
                    case ConsoleKey.Escape:
                        Log.Information("User is quitting.");
                        return -1;
                    case ConsoleKey.UpArrow:
                        //perform boundary checking
                        if (ThePlayer.X > 0)
                        {
                            //if the space moving upwards is a blank space then move up
                            if (GetTileAtPos(ThePlayer.X, ThePlayer.Y - 1).IsWalkable)
                            {
                                MovePlayer(PlayerMovement.Up);
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (ThePlayer.Y < Height - 1)
                        {
                            if (GetTileAtPos(ThePlayer.X, ThePlayer.Y + 1).IsWalkable)
                            {
                                MovePlayer(PlayerMovement.Down);
                            }
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (ThePlayer.X < Width - 1)
                        {
                            if (GetTileAtPos(ThePlayer.X + 1, ThePlayer.Y).IsWalkable)
                            {
                                MovePlayer(PlayerMovement.Right);
                            }
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (ThePlayer.X > 0)
                        {
                            if (GetTileAtPos(ThePlayer.X - 1, ThePlayer.Y).IsWalkable)
                            {
                                MovePlayer(PlayerMovement.Left);
                            }
                        }
                        break;
                }

            }


            //foreach player in players coming soon....
            ThePlayer.Update();

            MonsterMgr.UpdateAll(this);

            Console.SetCursorPosition(0, 0);

            return 0;
        }

        internal MapTile GetTileAtPos(int x, int y)
        {
            return Tiles[y, x];
        }
    }
}
