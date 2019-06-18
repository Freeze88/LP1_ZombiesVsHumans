using System;
using System.Collections.Generic;
using System.Threading;

namespace ZombiesVsHumans
{
    /// <summary>
    /// Part of the class Map
    /// </summary>
    public partial class Map
    {
        // Public enum responsible for the direction of the agents
        public enum ENUM_Direction : ushort
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        // Public enum responsible for continuing or stopping the game
        public enum ENUM_Simulation_Result : ushort
        {
            Success = 0,
            GameOver = 1,
            Quit = 2,
        }

        // Creates an array of MapPieces
        MapPiece[,] pieces;
        // Creates a list of Controllable Humans
        List<Human> players = new List<Human>();
        // Creates a list of Humans
        List<Human> NPCs = new List<Human>();
        // creates a list of zombies
        internal List<Zombie> zombies = new List<Zombie>();

        // Creates an int with the value of zero to switch between all the players
        int playerIndex = 0;
        uint turnNumber = 0;

        /// <summary>
        /// Contructs the map and the MapPieces with the values given
        /// </summary>
        /// <param name="mapWidth"> The max width of the map </param>
        /// <param name="mapHeight"> The max height of the map </param>
        /// <param name="playerCount"> The amount of Humans </param>
        /// <param name="playersToControl"> The amount of controllable humans </param>
        /// <param name="zombieCount"> The number of zombies </param>
        public Map(uint mapWidth, uint mapHeight, uint playerCount,
            uint playersToControl, uint zombieCount, uint turns)
        {
            turnNumber = turns;
            // Gives the pieces array declares de size of the map ( width, Height )
            pieces = new MapPiece[mapWidth, mapHeight];

            // A for cycle for the height of the map
            for (int y = 0; y < mapHeight; y++)
                // Another for the width of the map
                for (int x = 0; x < mapWidth; x++)
                    // Fills the board with empty spaces from the EmptySpace class
                    Add(new EmptySpace(new Vector2(x, y)));

            // A for cycle that runs by the amount of humans including the ones you control
            for (uint i = 0, j = 0; i < playerCount; i++, j++)
                // Goes to function PlacePlayer taking 2 bools
                PlacePlayer(j < playersToControl, i == 0);

            // A for cycle to place all the zombies 
            for (uint i = 0; i < zombieCount; i++)
                PlaceZombie();

            // Updates all the states of the humans and zombies
            Update();
        }

        /// <summary>
        /// Responsible for putting the players on the map
        /// </summary>
        /// <param name="canControl"> Defines if the human is controllable or not </param>
        /// <param name="isTurn"> Passes the bool into the Player class to control </param>
        private void PlacePlayer(bool canControl, bool isTurn)
        {
            // Gets a random X coordinate inside the bounds
            int posX = Mathf.RandomRange(0, pieces.GetLength(0) - 1);
            // Gets a random Y coordinate inside the bounds
            int posY = Mathf.RandomRange(0, pieces.GetLength(1) - 1);

            // Creates a new player with the coordinates created
            Human playa = new Human(new Vector2(posX, posY), canControl, isTurn);

            // If the human is controllable
            if (canControl)
            {
                // Adds that human to the player list
                players.Add(playa);

                // Adds OnPlayerInfected to the event handler OnInfected
                playa.OnInfected += OnPlayerInfected;
            }
            else
            {
                // Adds that human to the NPC list
                NPCs.Add(playa);
                // Adds OnNPCInfected to the event handler OnInfected
                playa.OnInfected += OnNPCInfected;
            }

            // Sends the human to the Add function
            Add(playa);

        }

        /// <summary>
        /// Removes the human from the list and stops executing OnCharacterMoved
        /// </summary>
        /// <param name="sender"> Takes in the desired human </param>
        private void OnPlayerInfected(Human sender)
        {
            // Removes the player from the list
            players.Remove(sender);
            // takes away OnCharacterMoved from the EventHandler
            sender.OnMoved -= OnCharacterMoved;
        }

        /// <summary>
        /// Removes the human from the list and stops executing OnCharacterMoved
        /// </summary>
        /// <param name="sender"> Takes in the desired human </param>
        private void OnNPCInfected(Human sender)
        {
            // Removes the player from the list
            NPCs.Remove(sender);
            // takes away OnCharacterMoved from the EventHandler
            sender.OnMoved -= OnCharacterMoved;
        }

        /// <summary>
        /// Places the zombie on a random position
        /// </summary>
        private void PlaceZombie()
        {
            // Creates to variables with the value of 0
            int x = 0;
            int y = 0;

            // Checks if the position on X and Y is not empty
            while (!(pieces[x, y] is EmptySpace))
            {
                // Gets a random X coordinate inside the bounds
                x = Mathf.RandomRange(1, pieces.GetLength(0) - 1);
                // Gets a random Y coordinate inside the bounds
                y = Mathf.RandomRange(1, pieces.GetLength(1) - 1);
            }

            // Creates a new zombie with the coordinates created
            Zombie zombie = new Zombie(new Vector2(x, y));
            // Adds the zombie to the zombie list
            Add(zombie);
        }

        /// <summary>
        /// Responsible for placing agents on the board
        /// </summary>
        /// <param name="piece"> Gets the desired agent </param>
        private void Add(MapPiece piece)
        {
            // Checks if the piece is an agent
            if (piece is Agents auxCharacter)
                // Takes away the onCharacterMoved from the event handler
                auxCharacter.OnMoved -= OnCharacterMoved;

            // Adds that piece to the array
            pieces[piece.Position.X, piece.Position.Y] = piece;

            // Checks agains if it is an agent
            if (piece is Agents character)
                // Adds back the OnCharacterMoved to the event handler
                character.OnMoved += OnCharacterMoved;

            // if the agent is a zombie and that zombie isn't there already
            if (piece is Zombie zombie && !zombies.Contains(zombie))
            {
                // Goes to Add function on zombie
                zombies.Add(zombie);
                // Goes to Propegate function and takes the current map
                zombie.Propegate(this);
            }
        }

        /// <summary>
        /// Cleans up the Map after the agents moved
        /// </summary>
        /// <param name="sender"> The current agent </param>
        /// <param name="direction"> The direction intended </param>
        /// <param name="oldPosition"> The position the agent moved from </param>
        private void OnCharacterMoved(Agents sender, ENUM_Direction direction, Vector2 oldPosition)
        {
            // Creates a new EmptySpace where the agent was
            EmptySpace space = new EmptySpace(oldPosition);
            // Sets that position to the Hash of the human
            space.SetPlayerHash(pieces[space.Position.X, space.Position.Y].PlayerHash);
            // Sets that position to the Hash of the Zombie
            space.SetZombieHash(pieces[space.Position.X, space.Position.Y].ZombieHash);
            // Adds that space back in with the Add function
            Add(space);

            // Sets that position of the target agent to the hash of the zombie
            sender.SetZombieHash(pieces[sender.Position.X, sender.Position.Y].ZombieHash);
            // Sets that position of the target agent to the hash of the human
            sender.SetPlayerHash(pieces[sender.Position.X, sender.Position.Y].PlayerHash);
            // Adds that agent into the field again
            Add(sender);
        }

        /// <summary>
        /// Static class to shuffle the lists
        /// Fisher-Yates shuffle
        /// </summary>
        /// <typeparam name="T"> The type of function T list </typeparam>
        /// <param name="list"> The list provided to it </param>
        public static void Shuffle<T>(IList<T> list)
        {
            // Creates a new random number
            Random rng = new Random();
            // Equals nObjects to the number of objects in the list
            int nObjects = list.Count;
            // runs through all the objects in the list except the last
            while (nObjects > 1)
            {
                // Subtracts 1 to nObjects
                nObjects--;

                // Creates an int new pos and gives it a value
                int newPos = rng.Next(nObjects + 1);
                // The value of the list is the newPos
                T value = list[newPos];
                // Switches newPos with nObjects
                list[newPos] = list[nObjects];
                // Switches the nObjects with the newPos
                list[nObjects] = value;
            }
        }
        /// <summary>
        /// Resposible for determening the status of the game
        /// </summary>
        /// <returns> The current state of the game </returns>
        public ENUM_Simulation_Result Simulate()
        {
            Shuffle(NPCs);
            Shuffle(zombies);

            // Defaults the value of the result to sucess
            ENUM_Simulation_Result simulationResult = ENUM_Simulation_Result.Success;

            // Draws the board
            Render();

            // Defines a string called input
            string input = "";

            // Checks if the number of humans total is more than 0
            if (players.Count > 0 || NPCs.Count > 0)
                do
                {
                    // Checks if there's controllable humans and they can move
                    if (players.Count == 0 || !CanMove)
                    {
                        // Defines the amount of delay between the simulations
                        int milliseconds = 100;
                        // Puts the thread to sleep for the defined amount of time
                        Thread.Sleep(milliseconds);
                        // Gives a random input to continue the program
                        ApplyInput(" ");
                    }
                    else
                    {
                        // Displays a message to the user
                        Console.Write("(w, s, a, d) To move the magenta player ; 'quit' to Exit : ");
                        // Expects a message from de user
                        input = Console.ReadLine();
                    }
                    // Runs the cycle until an expected input is given
                } while (!ApplyInput(input));

            // Switches between all the players
            SwitchTurn();

            turnNumber--;

            // Checks if there's no humans left
            if (players.Count == 0 && NPCs.Count == 0 || turnNumber <= 0)
                // Sets the result to GameOver
                simulationResult = ENUM_Simulation_Result.GameOver;

            // Checks if the user tried to quit
            if (input.ToLower() == "quit")
                // Sets the result to Quit
                simulationResult = ENUM_Simulation_Result.Quit;

            // Gives back the result
            return simulationResult;
        }

        /// <summary>
        /// Displays the map to the user
        /// </summary>
        public void Render()
        {
            // Creates a for loop the size of the array on Y
            for (uint y = 0; y < pieces.GetLength(1); y++)
            {
                // Creates a for loop the size of the array on X
                for (uint x = 0; x < pieces.GetLength(0); x++)
                    // Prints the information of that piece on MapPieced print function
                    pieces[x, y].Print();
                // Prints an empty line
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Checks if the input given matches the input wanted
        /// </summary>
        /// <param name="input"> A string provided for the check</param>
        /// <returns> A bool if the input is valid </returns>
        private bool ApplyInput(string input)
        {
            // If it's simulation mode it accepts any input
            if (players.Count == 0 || !CanMove)
                return true;

            // Checks w,a,s,d and moves that player to the intended direction
            if (input.ToLower() == "w")
                return players[playerIndex].Move(this, ENUM_Direction.Up);
            else if (input.ToLower() == "s")
                return players[playerIndex].Move(this, ENUM_Direction.Down);
            else if (input.ToLower() == "a")
                return players[playerIndex].Move(this, ENUM_Direction.Left);
            else if (input.ToLower() == "d")
                return players[playerIndex].Move(this, ENUM_Direction.Right);

            // returns true as default
            return true;
        }

        /// <summary>
        /// Switches between the players 
        /// </summary>
        private void SwitchTurn()
        {
            // Checks if there's no controllable humans
            if (players.Count == 0)
            {
                // Apllies the AI of the agent and returns
                ApplyAI();
                return;
            }

            // Sets the player turn to false (Starts with player 0)
            players[playerIndex].SetTurn(false);

            // Increments the player index
            playerIndex++;

            // Checks if the playerIndex is bigger or the same as the amount of players
            if (playerIndex >= players.Count)
            {
                // Sets the Index back to 0
                playerIndex = 0;
                // Applies the AI to the remaining humans
                ApplyAI();
            }
            else
                // Updates the game state
                Update();

            // if the the amount of players is more than one
            if (players.Count > 0)
                // Sets the SetTurn bool back to true
                players[playerIndex].SetTurn(true);
        }

        /// <summary>
        /// Goes through all the agents and moves them
        /// Also checks if any human is infectable
        /// </summary>
        private void ApplyAI()
        {
            // Goes through all the humans on the NPC'S
            foreach (Human player in NPCs.ToArray())
                // Moves the player taking in the Map
                player.Move(this);
            // Calculates the hashes taking in the map with the player information
            MapPiece.CalculateHash(this);

            // Goes through all the zombies on the zombies
            foreach (Zombie zombie in zombies.ToArray())
                //Moves the zombie taking in the Map
                zombie.Move(this);
            // Calculates the hashes taking in the mao with the zombie information
            MapPiece.CalculateHash(this);

            // Checks if any human is infectable on the map
            Zombie.Infect(this);
        }

        /// <summary>
        /// Calculates the Hashes with the current information
        /// </summary>
        private void Update()
        {
            // Calculates the hashes of the current map
            MapPiece.CalculateHash(this);
            // checks if any human is infectable on the map
            Zombie.Infect(this);

            // if the Indez is bigger than the total of players
            if (playerIndex >= players.Count)
                // Sets the index to 0
                playerIndex = 0;
        }

        /// <summary>
        /// Retrives a position (X,Y)
        /// </summary>
        /// <param name="dir"> The direction intended </param>
        /// <param name="position"> The position of the piece (X, Y) </param>
        /// <returns></returns>
        internal MapPiece GetPiece(ENUM_Direction dir, Vector2 position)
        {
            // Switch case with the direction (dir) given
            switch (dir)
            {
                // Case up
                case ENUM_Direction.Up:
                    // Is on the upper edge
                    if (position.Y == 0)
                        // The new position on Y is the bottom Y
                        return pieces[position.X, pieces.GetLength(1) - 1];
                    else
                        // Lowers the Y position by 1
                        return pieces[position.X, position.Y - 1];
                // Case down
                case ENUM_Direction.Down:
                    // Is on lower edge
                    if (position.Y == pieces.GetLength(1) - 1)
                        // The new position on Y is the top Y
                        return pieces[position.X, 0];
                    else
                        // Augments the Y position by 1
                        return pieces[position.X, position.Y + 1];
                // Case left
                case ENUM_Direction.Left:
                    // Is on the left edge
                    if (position.X == 0)
                        // The new position on X is the most right X
                        return pieces[pieces.GetLength(0) - 1, position.Y];
                    else
                        // Lowers the X position by 1
                        return pieces[position.X - 1, position.Y];
                // Case right
                case ENUM_Direction.Right:
                    // Is on the right edge
                    if (position.X == pieces.GetLength(0) - 1)
                        // The new position on X is the most left X
                        return pieces[0, position.Y];
                    else
                        // Augments the X position by 1
                        return pieces[position.X + 1, position.Y];
            }
            // Default return null (Not supposed to get here anyway)
            return null;
        }

        /// <summary>
        /// Returns a bool if the player is capaple of moving atleast in one direction
        /// </summary>
        private bool CanMove
        {
            get
            {
                // Goes into GetPiece and cheks every possible move
                MapPiece topPiece = GetPiece(ENUM_Direction.Up, players[playerIndex].Position);
                MapPiece BottomPiece = GetPiece(ENUM_Direction.Down, players[playerIndex].Position);
                MapPiece leftPiece = GetPiece(ENUM_Direction.Left, players[playerIndex].Position);
                MapPiece rightPiece = GetPiece(ENUM_Direction.Right, players[playerIndex].Position);

                // Returns true if any of the possible moves has an EmptySpace in it
                return topPiece is EmptySpace || BottomPiece is EmptySpace
                    || leftPiece is EmptySpace || rightPiece is EmptySpace;
            }
        }
    }
}
