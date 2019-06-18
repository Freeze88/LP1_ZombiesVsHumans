using System;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Main program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Starts the aplication and runs it
        /// </summary>
        /// <param name="args"> Accepts a string</param>
        static void Main(string[] args)
        {
            // Initializes the application by asking how to build the map
            Application.Initialize(args);

            // Runs the application until the game ends or the user quits
            Application.Run();
        }
    }
}
