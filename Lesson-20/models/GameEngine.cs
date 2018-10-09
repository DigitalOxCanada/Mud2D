using Figgle;
using Newtonsoft.Json;
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
    public static class GameEngine
    {
        const string LevelsPath = @"maps\Levels.yaml";
        public static readonly int DEFAULTFOWRANGE = 1;

        private static bool Dirty { get; set; }
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int MaxPlayers { get; set; }
        public static bool IsFOWEnabled { get; set; }
        public static int FOWRange { get; set; }

        public static MessageBoard MessageBrd { get; set; }
        public static MapTile[,] Tiles { get; set; }
        public static Player ThePlayer { get; set; }   //NOTE single for now, eventually we use an array or list of players
        public static MonsterManager MonsterMgr { get; set; }
        public static ObjectManager ObjectMgr { get; set; }
        public static ScoreCard ScoreCard { get; set; }

        public static LevelYaml CurrentLevel { get; set; }
        public static List<LevelYaml> Levels { get; set; }

        public enum GameStateType {
            MainMenu,
            InGame,
            Quit
        }

        public static GameStateType GameState;

        static GameEngine()
        {
            Dirty = true;  //when loading a new map we trigger redrawing map
            MaxPlayers = 1;
            FOWRange = DEFAULTFOWRANGE;
            IsFOWEnabled = true;
            ThePlayer = new Player();
            MonsterMgr = new MonsterManager();
            ObjectMgr = new ObjectManager();
            ScoreCard = new ScoreCard();
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

        static public void LoadLevelData()
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

        static public void StartSavedLevel(int level)
        {
            Width = CurrentLevel.Width;
            Height = CurrentLevel.Height;

            Random randgen = new Random();
            MonsterMgr.ClearMonsters();
            MonsterMgr.AddMonsters(randgen.Next(2, level + 3));

            ObjectMgr.AddObjectsToMap(randgen.Next(3, level + 13));

            ThePlayer.Dirty = true;
            Dirty = true;
        }


        static public void StartNewLevel(int level)
        {
            CurrentLevel = Levels.Where(p => p.Level == level).SingleOrDefault();
            if (CurrentLevel == null)
            {
                string msg = $"Failed to Start Level {level}.";
                Log.Fatal(msg);
                throw new Exception(msg);   //we need to stop here since the game is unplayable if this happens
            }

            Width = CurrentLevel.Width;
            Height = CurrentLevel.Height;
            Tiles = null;
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


            Random randgen = new Random();
            MonsterMgr.ClearMonsters();
            MonsterMgr.AddMonsters(randgen.Next(2, level + 3));

            ObjectMgr.AddObjectsToMap(randgen.Next(3, level + 13));

            ThePlayer.Dirty = true;
            Dirty = true;
        }



        /// <summary>
        /// Pass a text symbol and the appropriate type gets created and added to the map
        /// </summary>
        /// <param name="symbol"></param>
        static MapTile CreateTile(char symbol, int x, int y)
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
                case 'U':
                    {
                        MapTileStairs d = new MapTileStairs(-1);
                        d.X = x;
                        d.Y = y;
                        Tiles[y, x] = d;
                    }
                    break;
                //stairs down tile
                case 'D':
                    {
                        MapTileStairs d = new MapTileStairs(1);
                        d.X = x;
                        d.Y = y;
                        Tiles[y, x] = d;
                    }
                    break;
            }
            return Tiles[y, x];
        }

        static void MovePlayer(PlayerMovement dir)
        {
            ThePlayer.Dirty = true;
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

            //TODO lets see what's on this tile
            //it could be an object or stairs which IsWalkable but an action should occur now

            CheckTileForAction(ThePlayer.X, ThePlayer.Y);
        }

        static void CheckTileForAction(int x, int y)
        {
            //if this tile is stairs, perform stairs action
            if (Tiles[y, x].GetType() == typeof(MapTileStairs))
            {
                var tile = ((MapTileStairs)Tiles[y, x]);
                var str = tile.Direction < 0 ? "up" : "down";
                MessageBrd.Add($"You are on stairs going {str}.");

                StartNewLevel(CurrentLevel.Level + tile.Direction);
                Dirty = true;
                ThePlayer.Dirty = true;
            }
            else if(Tiles[y,x].ActionObject!=null)
            {
                Tiles[y, x].ActionObject.OnActionEnter();
            }

        }

        static void UpdateFogOfWar()
        {
            //if the user has the Torch in inventory then increase FOWRange
            var hastorch = ThePlayer.Inventory.Where(p => p.Name == "Torch").Count();  //can have more than 1 in inventory
            if (hastorch>0)
            {
                FOWRange = DEFAULTFOWRANGE+hastorch;
            }

            //added range for things like Torches, lanterns, etc. that give a bigger relief from FOW
            for (int range = 1; range <= FOWRange; range++)
            {
                for (int y = ThePlayer.Y - range; y <= ThePlayer.Y + range; y++)
                {
                    for (int x = ThePlayer.X - range; x <= ThePlayer.X + range; x++)
                    {
                        if (x > -1 && y > -1 && x < Width && y < Height)
                        {
                            Tiles[y, x].ClearFOG();
                        }
                    }
                }
            }
        }

        private static void SaveGame()
        {
            string playerjson = JsonConvert.SerializeObject(ThePlayer);
            string leveljson = JsonConvert.SerializeObject(CurrentLevel);
            string tilesjson = JsonConvert.SerializeObject(Tiles);
            File.WriteAllText("save-player.json", playerjson);
            File.WriteAllText("save-level.json", leveljson);
            File.WriteAllText("save-tiles.json", tilesjson);
        }


        private static void LoadLastGame()
        {
            LoadLevelData();
            CurrentLevel = JsonConvert.DeserializeObject<LevelYaml>(File.ReadAllText("save-level.json"));
            var Tiles = JsonConvert.DeserializeObject<MapTile[,]>(File.ReadAllText("save-tiles.json"));

            StartSavedLevel(CurrentLevel.Level);
                
            ThePlayer = JsonConvert.DeserializeObject<Player>(File.ReadAllText("save-player.json"));

            ScoreCard.Dirty = true;
        }


        private static void InitializeGame()
        {
            LoadLevelData();
            StartNewLevel(1);

            MapTile blank = GetRandomTileSpace();
            //TODO make a moveto method instead of setting properties.
            ThePlayer.X = blank.X;
            ThePlayer.Y = blank.Y;
            ScoreCard.Dirty = true;
        }


        /// <summary>
        /// Draw the map
        /// </summary>
        static public int Update()
        {
            switch(GameState)
            {
                case GameStateType.MainMenu:
                    if (Dirty)
                    {
                        Console.Clear();
                        Console.WriteLine(FiggleFonts.Ogre.Render("Mud2D"));
#if DEBUG
                        Console.WriteLine("DEBUG version");
                        Log.Information("DEBUG version");
#elif RELEASE
            Console.WriteLine("RELEASE version");
            Log.Information("RELEASE version");
#endif
                        Console.WriteLine("------===== Main Menu =====-------");
                        Console.WriteLine("Press S to start game");
                        Console.WriteLine("Press L to load last save game");
                        Console.WriteLine("Press ESC to exit");
                        Dirty = false;
                    }

                    //if there is a keypress available to capture, take it and perform its action
                    if (Console.KeyAvailable)
                    {
                        var ch = Console.ReadKey(true).Key;
                        switch (ch)
                        {
                            case ConsoleKey.Escape:
                                GameState = GameStateType.Quit;
                                Log.Information("User is quitting game.");
                                return -1;
                            case ConsoleKey.L:
                                GameState = GameStateType.InGame;
                                LoadLastGame();
                                break;
                            case ConsoleKey.S:
                                GameState = GameStateType.InGame;
                                InitializeGame();
                                break;
                        }
                    }
                    break;

                case GameStateType.InGame:
                    if (ThePlayer.Life < 1)
                    {
                        Console.Clear();
                        MessageBrd.Add($"You have DIED!");
                        MessageBrd.Update();
                        return -1;
                    }

                    var origRow = Console.CursorTop;
                    var origCol = Console.CursorLeft;

                    UpdateFogOfWar();

                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            Tiles[y, x].Draw();
                        }
                    }
                    Dirty = false;

                    //foreach player in players coming soon....
                    ThePlayer.Update();

                    MonsterMgr.UpdateAll();

                    MessageBrd.Update();

                    ScoreCard.Update();


                    //if there is a keypress available to capture, take it and perform its action
                    if (Console.KeyAvailable)
                    {
                        var ch = Console.ReadKey(true).Key;
                        switch (ch)
                        {
                            case ConsoleKey.Escape:
                                GameState = GameStateType.MainMenu;
                                Dirty = true;
                                SaveGame();
                                Log.Information("User is quitting game, going back to main menu.");
                                break;
                            case ConsoleKey.F2:
                                CheatToggleFOW();
                                break;
                            case ConsoleKey.UpArrow:
                                PlayerTryMoveOffsetTo(0, -1);
                                break;
                            case ConsoleKey.DownArrow:
                                PlayerTryMoveOffsetTo(0, 1);
                                break;
                            case ConsoleKey.RightArrow:
                                PlayerTryMoveOffsetTo(1, 0);
                                break;
                            case ConsoleKey.LeftArrow:
                                PlayerTryMoveOffsetTo(-1, 0);
                                break;
                        }

                    }

                    //Console.SetCursorPosition(0, 0);

                    return 0;
            }
            return 0;
        }

        private static void PlayerTryMoveOffsetTo(int xoffset, int yoffset)
        {
            //this is the proposed new x,y of the player if move is successful.
            int newx = ThePlayer.X + xoffset;
            int newy = ThePlayer.Y + yoffset;
            
            //perform boundary checking of new location, if it fails then we can't move there just because of boundary issues
            if (newx > -1 && newx < Width && newy>-1 && newy<Height )
            {
                //get the tile that is at the new location
                MapTile t = GetTileAtPos(newx, newy);

                //if the space moving upwards is a blank space then move up
                if (t.IsWalkable)
                {
                    ThePlayer.Dirty = true;
                    Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = true;  //the space we just moved from
                    ThePlayer.LastX = ThePlayer.X;
                    ThePlayer.LastY = ThePlayer.Y;
                    ThePlayer.X = newx;
                    ThePlayer.Y = newy;
                    Tiles[ThePlayer.Y, ThePlayer.X].IsWalkable = false; //make new tile not walkable because you are on it

                    CheckTileForAction(ThePlayer.X, ThePlayer.Y);
                }
                else
                {
                    //if we can't walk onto this space then what is in this space? a wall, door, monster, etc.?
                    //walls do nothing, doors we can check for keys, monsters we attack.
                    var type = GetTileAtPos(newx, newy).GetType();
                    if (type==typeof(MapTileWall))
                    {
                        //do nothing, its a wall
                    }
                    else if(type==typeof(MapTileSpace))
                    {
                        //so if its a space and its not walkable, probably a monster here.
                        var monster = MonsterMgr.GetMonsterAt(newx, newy);
                        if(monster.IsAlive)
                        {
                            var maxdamage = ThePlayer.CanDealDamage();
                            //lets hit the monster for damage
                            monster.TakeDamage(maxdamage);
                            MessageBrd.Add($"Monster took {maxdamage} damage.");
                            if(!monster.IsAlive)
                            {
                                MessageBrd.Add($"Monster is DEAD!");
                            }
                            MonsterMgr.PruneDeadMonsters();
                        }
                    }
                    
                }
            } // end of map boundary check

        }

        internal static void CheatToggleFOW()
        {
            IsFOWEnabled = !IsFOWEnabled;
            Dirty = true;
            foreach(var tile in Tiles)
            {
                tile.FOW = (IsFOWEnabled == true ? DEFAULTFOWRANGE : 0);
                tile.Dirty = true;
            }
        }

        static MapTile GetTileAtPos(int x, int y)
        {
            return Tiles[y, x];
        }

        static public MapTile GetRandomTileSpace()
        {
            Random randgen = new Random();

            var blanks = (from t in Tiles.Cast<MapTile>() where t.GetType() == typeof(MapTileSpace) && t.ActionObject == null select t).ToArray();
            var blank = blanks[randgen.Next(blanks.Length)];

            return blank;
        }

    }

}
