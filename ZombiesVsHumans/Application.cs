using System;

namespace ZombiesVsHumans
{
    public static class Application
    {
        // Creates and instance of map
        static Map map;

        // Initializes the program taking the parameters it needs.
        public static void Initialize(string[] args)
        {
            // Initializes the width as 0
            uint mapWidth = 0;
            // Initializes the height as 0
            uint mapHeight = 0;
            // Initializes the number of zombies as 0
            uint zombieCount = 0;
            // Initializes the number of humans as 0
            uint humanCount = 0;
            // Initializes the number of players as 0
            uint playerControlCount = 0;
            // Initializes the number of turns as 0
            uint turns = 0;

            // Checks the 1st string provided
            if (args[0] == "-x")
                // Converts the next string and give it to the width
                mapWidth = Convert.ToUInt32(args[1]);
            // Checks for the 3rd string provided
            if (args[2] == "-y")
                // Converts the next string and gives it to the height
                mapHeight = Convert.ToUInt32(args[3]);
            // Checks the 5th string provided 
            if (args[4] == "-z")
                // Converts the next string and gives it to the number of zombies
                zombieCount = Convert.ToUInt32(args[5]);
            // Checks the 7th string provided
            if (args[6] == "-h")
                // Converts the next string and gives it to the amount of humans
                humanCount = Convert.ToUInt32(args[7]);
            // Checks the 9th string provided
            if (args[8] == "-H")
                // Converts the next string and gives it to the amount of players
                playerControlCount = Convert.ToUInt32(args[9]);
            // Checks the 11th string provided
            if (args[10] == "-t")
                // Converts the next string and gives it to the amount of turns
                turns = Convert.ToUInt32(args[11]);

            // Creates a new map sending the characteristics the user gave
            map = new Map(mapWidth, mapHeight, humanCount, playerControlCount, zombieCount, turns);
        }

        // Responsible for the main loop of the game
        public static void Run()
        {
            // Creates a local variable result and equals it to the ma enumerator "result"
            Map.ENUM_Simulation_Result result = Map.ENUM_Simulation_Result.Success;

            // While the game is not over or the player quit
            do
            {
                // Clears everything on the console
                Console.Clear();
                // Goes to the Map class to draw and check the AIs
                result = map.Simulate();
            }
            while (result == Map.ENUM_Simulation_Result.Success);

            // Checks if the game has ended
            if (result == Map.ENUM_Simulation_Result.GameOver)
            {
                // Clears the console
                Console.Clear();
                // Shows the player the final board without applying AI
                map.Render();
                // Changes the font color to red
                Console.ForegroundColor = ConsoleColor.Red;
                // Prints an empty line
                Console.WriteLine();
                // Shows a message to the user
                Console.WriteLine("GAME OVER!!");
                // Turns the color back to white
                Console.ForegroundColor = ConsoleColor.White;
                // Asks for any input so that the console doesn't close automatically
                Console.ReadKey();
            }
        }
    }
}
