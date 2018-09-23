using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mud2D.models
{
    public class GameEngine
    {
        const string LevelsPath = @"maps\Levels.yaml";

        private bool NeedsRedrawing { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }
        public string Author { get; set; }

        public MessageBoard MessageBrd { get; set; }
        public MapTile[,] Tiles { get; set; }
        public Player ThePlayer { get; set; }   //NOTE single for now, eventually we use an array or list of players
        public MonsterManager MonsterMgr { get; set; }

        public LevelYaml CurrentLevel { get; set; }
        public List<LevelYaml> Levels { get; set; }

        public GameEngine()
        {
            NeedsRedrawing = true;  //when loading a new map we trigger redrawing map
            MaxPlayers = 1;
            ThePlayer = new Player();
            MonsterMgr = new MonsterManager();
            MessageBrd = new MessageBoard();
            MessageBrd.Add("Welcome to Mud2D");
        }

        public class LevelYaml
        {
            [YamlMember(Alias = "level")]
            public int Level { get; set; }

            [YamlMember(Alias = "width")]
            public int Width { get; set; }

            [YamlMember(Alias = "height")]
            public int Height { get; set; }

            [YamlMember(Alias = "map")]
            public string Map { get; set; }
        }

        public void LoadLevelData()
        {
            //create yaml deserializer
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            var fileContents = File.ReadAllText(LevelsPath);
            var input = new StringReader(fileContents);
            var parser = new Parser(input);
            parser.Expect<StreamStart>();
            Levels = new List<LevelYaml>();
            while (parser.Accept<DocumentStart>())
            {
                var doc = deserializer.Deserialize<LevelYaml>(parser);
                //there is a wierd effect, when the map is read in as a single string it reads in a space after the first line and every line after
                //so when parsing the map as a single string that extra space needs to be skipped.
                Levels.Add(doc);
            }

        }

        public void StartLevel(int level)
        {
            CurrentLevel = Levels.Where(p => p.Level == level).SingleOrDefault();
            if(CurrentLevel == null)
            {
                string msg = $"Failed to Start Level {level}.";
                Log.Fatal(msg);
                throw new Exception(msg);   //we need to stop here since the game is unplayable if this happens
            }

            //string[] mapLines = System.IO.File.ReadAllLines(mapFilename); //old method
            Width = CurrentLevel.Width;
            Height = CurrentLevel.Height;
            Tiles = new MapTile[CurrentLevel.Height, CurrentLevel.Width];

            for (int yPos = 0; yPos < CurrentLevel.Height; yPos++)
            {
                //create the Map 2d array data from the current line char by char
                for (int xPos = 0; xPos < CurrentLevel.Width; xPos++)
                {
                    //this calculates row * width of row + column (+1 per row [0 indexed] to adjust for spaces at the end of rows from yaml deserialization)
                    int strpos = yPos * CurrentLevel.Width + xPos + (yPos);
                    CreateTile(CurrentLevel.Map[strpos], xPos, yPos);
                }
            }

            //TODO place player in random available space like monsters are
            ThePlayer.X = 10;
            ThePlayer.Y = 10;

            Random randgen = new Random();
            MonsterMgr.AddMonster(Tiles, randgen.Next(2,level+3));
            //MonsterMgr.AddMonster(Tiles, randgen.Next(1, 2));
        }



        /// <summary>
        /// Pass a text symbol and the appropriate type gets created and added to the map
        /// </summary>
        /// <param name="symbol"></param>
        public MapTile CreateTile(char symbol, int x, int y)
        {
            switch (symbol)
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
            return Tiles[y, x];
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

            MessageBrd.Update();

            Console.SetCursorPosition(0, 0);

            return 0;
        }

        internal MapTile GetTileAtPos(int x, int y)
        {
            return Tiles[y, x];
        }
    }

}
