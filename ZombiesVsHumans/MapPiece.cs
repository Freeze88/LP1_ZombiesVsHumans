using System;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal abstract class MapPiece
        {
            // Sets a variable for color
            protected ConsoleColor color;
            // Sets a default prefix value
            protected char prefix = ' ';

            /// <summary>
            /// Constructor of the MapPiece class
            /// </summary>
            /// <param name="position"> The position of the piece </param>
            /// <param name="prefix"> An empty char for later printing </param>
            /// <param name="color"> The desired color </param>
            public MapPiece(Vector2 position, char prefix = ' ', ConsoleColor color = ConsoleColor.White)
            {
                // Sets all the class values to the ones provided
                Position = position;
                this.prefix = prefix;
                this.color = color;
                // Gives the max value to the player and zombie hash variables
                PlayerHash = float.MaxValue;
                ZombieHash = float.MaxValue;
            }

            // Converts the char prefix to a string
            public override string ToString() => prefix.ToString();

            // Gives the human Hash a new value
            public void SetPlayerHash(float value) => PlayerHash = value;

            // Gives the zombie Hash a new value
            public void SetZombieHash(float value) => ZombieHash = value;

            /// <summary>
            /// Calculates the hash around the mapPieces
            /// </summary>
            /// <param name="map"> The current state of the map </param>
            public static void CalculateHash(Map map)
            {
                // For loop for the map Lenght
                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    // For loop for the map Height
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                        // Sets the player Hash to the max value
                        map.pieces[x1, y1].SetPlayerHash(float.MaxValue);

                // Another for loop for the map lenght
                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    // Another for loop for the map height
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                    {
                        // Checks if the mapPiece is a zombie
                        bool isZombie = map.pieces[x1, y1] is Zombie;
                        // Checks if the mapPiece is a human
                        bool isPlayer = map.pieces[x1, y1] is Human;

                        // If it is a zombie or a player
                        if (isZombie || isPlayer)
                        {
                            // Creates an a bidemensional array
                            uint[] playerPos = new uint[] { x1, y1 };

                            // Two for loops for x and y of the map
                            for (uint y = 0; y < map.pieces.GetLength(1); y++)
                                for (uint x = 0; x < map.pieces.GetLength(0); x++)
                                {
                                    // Equals the Ypos to the Y of the loop
                                    float yPos = (float)y;
                                    // Checks if the distance between the player position and the current x and y
                                    // with the distance of the player and the X and Y but on the lower edge 
                                    if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x, y - map.pieces.GetLength(1), playerPos[0], playerPos[1]))
                                        // Gives the Y pos the value of Y minus the height of the map
                                        yPos = y - map.pieces.GetLength(1);
                                    // Checks if the distance between the player position and the current x and y
                                    // with the distance of the player and the X and Y but on the upper edge 
                                    else if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x, y + map.pieces.GetLength(1), playerPos[0], playerPos[1]))
                                        // Gives to Ypos the absolute value of Y plus the height of the map
                                        yPos = Math.Abs(y + map.pieces.GetLength(1));

                                    // Equals Xpos to the X of the loop
                                    float xPos = x;
                                    // Cheks if the distance between the player position and the current x and y
                                    // with the distance of the player on X and Y but on the left edge
                                    if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x - map.pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                        // Gives xPos the value of the x minus the lenght of the map
                                        xPos = x - map.pieces.GetLength(0);
                                    // Cheks if the distance between the player position and the current x and y
                                    // with the distance of the player on X and Y but on the left edge
                                    else if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x + map.pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                        // Gives the xPos the value of x plus the length of the map
                                        xPos = x + map.pieces.GetLength(0);

                                    // Creates 2 new variables and equals them to the players coordinates
                                    float pxPos = (float)playerPos[0];
                                    float pyPos = (float)playerPos[1];

                                    // Checks the distance between the Player coordinates the the xPos and Ypos
                                    float min = (float)Math.Sqrt(Math.Pow(pxPos - xPos, 2) + Math.Pow(pyPos - yPos, 2));

                                    // If it's a human
                                    if (isPlayer)
                                        // Sets the value of the human hash to the minumum value between the min and the current human hash
                                        map.pieces[x, y].SetPlayerHash(Mathf.Min(min, map.pieces[x, y].PlayerHash));

                                    // if it's a zombie
                                    if (isZombie)
                                        // Sets the value of the zombie hash to the minumum value between the min and the current zombie hash
                                        map.pieces[x, y].SetZombieHash(Mathf.Min(min, map.pieces[x, y].ZombieHash));
                                }
                        }
                    }
            }

            /// <summary>
            /// Displays to the user the agents on the field
            /// </summary>
            public virtual void Print()
            {
                // Gives the variable color the current color
                ConsoleColor color = Console.ForegroundColor;

                // Sets the font color to the color given
                Console.ForegroundColor = this.color;
                // Displays the prefix given to the class
                Console.Write("  " + prefix + "  ");

                // Reverts the color to its original color
                Console.ForegroundColor = color;
            }

            // Gets the player Hash
            public float PlayerHash { get; private set; }
            // Gets the Zombie Hash
            public float ZombieHash { get; private set; }
            // Gets and sets the position of the agent
            public Vector2 Position { get; protected set; }
        }
    }
}
