using System;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal partial class Zombie : Agents
        {
            /// <summary>
            /// Constructor of the Zombie class
            /// </summary>
            /// <param name="position"> The current position of the zombie </param>
            public Zombie(Vector2 position)
                : base(position, 'z', ConsoleColor.Red)
            {
            }

            /// <summary>
            /// Checks if any of the surrounding players is a human
            /// </summary>
            /// <param name="map"> The current state of the map </param>
            public void Propegate(Map map)
            {
                // Gets the pieces around the zombie
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, Position);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, Position);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, Position);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, Position);

                // If the top position has a human
                if (topPiece is Human playa1)
                {
                    // Infects the human
                    playa1.Infect();
                    // Adds a new zombie in the place of the human
                    map.Add(new Zombie(new Vector2(topPiece.Position)));
                }

                // If the bottom position has a human
                if (bottomPiece is Human playa2)
                {
                    // Infects the human
                    playa2.Infect();
                    // Adds a zombie in the place of the human
                    map.Add(new Zombie(new Vector2(bottomPiece.Position)));
                }

                // If the Right position has a human
                if (rightPiece is Human playa3)
                {
                    // Infects the human
                    playa3.Infect();
                    // Adds a Zombie in the place of the human
                    map.Add(new Zombie(new Vector2(rightPiece.Position)));
                }

                // If the left position has a human
                if (leftPiece is Human playa4)
                {
                    // Infects the human
                    playa4.Infect();
                    // Adds a zombie in the place of the human
                    map.Add(new Zombie(new Vector2(leftPiece.Position)));
                }
            }

            /// <summary>
            /// Goes through all the zombies and checks if they have humans near
            /// </summary>
            /// <param name="map"> The current state of the map </param>
            public static void Infect(Map map)
            {
                // Goes through all the zombies
                foreach (Zombie zombie in map.zombies.ToArray())
                    // Checks if they are near a human
                    zombie.Propegate(map);
            }

            /// <summary>
            /// Moves the zombie to the desired position
            /// </summary>
            /// <param name="map"> Gets the current state of the map </param>
            public override void Move(Map map)
            {
                // Gives the enum direction the value given by GetDirectionFromHash of the character class
                ENUM_Direction direction = GetDirectionFromHash(map, Position, true);

                // Goes to MoveToDirection taking the new direction and the map
                MoveToDirection(map, direction);
            }
        }
    }
}
