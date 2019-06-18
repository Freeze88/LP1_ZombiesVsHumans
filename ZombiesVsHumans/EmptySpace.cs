using System;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        // Responsible for managing the empty spaces and printing them
        internal class EmptySpace : MapPiece
        {
            /// <summary>
            /// Empty constructor that uses the MapPiece constructor as base
            /// </summary>
            /// <param name="position"> A Vector2 with the position of the mapPiece being checked </param>
            public EmptySpace(Vector2 position)
                : base(position, ' ', ConsoleColor.White) { }

            /// <summary>
            /// Responsible for printing the empty spaces on the map
            /// </summary>
            public override void Print()
            {
                // Creates a color variable and gives it the current color
                ConsoleColor color = Console.ForegroundColor;

                // Gives the color the value given by it's base class
                Console.ForegroundColor = this.color;

                // -- UNCOMMENT TO SHOW THE VALUES OF THE HASHES OF THE ZOMBIE AI--//
                //Console.Write(string.Format(" {0:F2} ", ZombieHash));
                // -- UNCOMMENT TO SHOW THE VALUES OF THE HASHES OF THE HUMAN AI--//
                //Console.Write(string.Format(" {0:F2} ", PlayerHash));

                // Prints a dot in the Empty spaces
                Console.Write("  .  ");

                // Resets the color of the font to the old one
                Console.ForegroundColor = color;
            }
        }
    }
}
