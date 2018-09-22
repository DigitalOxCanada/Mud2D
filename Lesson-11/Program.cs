using Figgle;
using Mud2D.models;
using Serilog;
using System;

namespace DigitalOx
{
    static public class Mud2DGame
    {
        static Map TheMap { get; set; }

        /// <summary>
        /// This is the main application entry point
        /// </summary>
        /// <param name="args">command line arguments passed into here</param>
        static void Main(string[] args)
        {
            //Log is a static class so it can be called from anywhere.
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

            Log.Information("Welcome to Mud2D!");

            Intro();

            InitializeMap("maps/map1.txt");

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

            GameLoop();

            Log.CloseAndFlush();
        }

        private static void Intro()
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
        }

        /// <summary>
        /// This is the main game loop handles keyboard input
        /// </summary>
        private static void GameLoop()
        {
            bool running = true;

            do
            {
                int retmsg = TheMap.Update();
                switch (retmsg)
                {
                    case -1:
                        running = false;
                        break;
                }
            } while (running);
        }


        private static void InitializeMap(string mapFilename)
        {
            TheMap = new Map(mapFilename);
            Log.Information("Map is loaded...size: [{Width} x {Height}]", TheMap.Width, TheMap.Height);
            Log.Information("Player 1 location: [{X}, {Y}]", TheMap.ThePlayer.X + 1, TheMap.ThePlayer.Y + 1);
        }

    }
}
