using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        // The mother class to the Player and Zombie classes
        internal abstract class Agents : MapPiece
        {
            public delegate void MoveEventHandler(Agents sender, ENUM_Direction direction, Vector2 oldPosition);

            public event MoveEventHandler OnMoved;

            /// <summary>
            /// Empty constructor for a character
            /// </summary>
            /// <param name="position"> a Vector 2 (x,y) </param>
            /// <param name="prefix"> The first letter of the agent </param>
            /// <param name="color"> Color given to distinguish the agents </param>
            public Agents(Vector2 position, char prefix, ConsoleColor color = ConsoleColor.DarkBlue)
                : base(position, prefix, color)
            {
            }

            /// <summary>
            /// Takes in a position and the map and moves the agent in a direction
            /// </summary>
            /// <param name="map"> Information about the current state of the map </param>
            /// <param name="position"> A vector 2 (x,y) </param>
            /// <param name="isZombie"> A check to see if its a zombie or human </param>
            /// <returns> The enum of the desired direction </returns>
            protected ENUM_Direction GetDirectionFromHash(Map map, Vector2 position, bool isZombie)
            {
                // Declares rPiece as null
                MapPiece rPiece = null;
                // Gets the cordinates that the agent should move to
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, position);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, position);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, position);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, position);

                // Makes the default value of rPiece the coordinates for going up
                rPiece = topPiece;

                // If the character is a zombie
                if (isZombie)
                {
                    // Checks the hash values of the player to see if any of the positions is closer to the humans
                    // if it is makes rPiece the position intended
                    rPiece = rPiece.PlayerHash < bottomPiece.PlayerHash ? rPiece : bottomPiece;
                    rPiece = rPiece.PlayerHash < leftPiece.PlayerHash ? rPiece : leftPiece;
                    rPiece = rPiece.PlayerHash < rightPiece.PlayerHash ? rPiece : rightPiece;
                }
                else
                {
                    // Checks the hash values of the zombies to see the place further from their current position
                    // if it is makes rPiece the position intended
                    rPiece = rPiece.ZombieHash > bottomPiece.ZombieHash ? rPiece : bottomPiece;
                    rPiece = rPiece.ZombieHash > leftPiece.ZombieHash ? rPiece : leftPiece;
                    rPiece = rPiece.ZombieHash > rightPiece.ZombieHash ? rPiece : rightPiece;
                }

                // If the rPiece has the same value has the bottom piece returns the direction down
                if (rPiece == bottomPiece)
                    return ENUM_Direction.Down;
                // If the rPiece has the same value has the left piece returns the direction left
                else if (rPiece == leftPiece)
                    return ENUM_Direction.Left;
                // If the rPiece has the same value has the right piece returns the direction right
                else if (rPiece == rightPiece)
                    return ENUM_Direction.Right;
                // If none of the conditions are met it defaults to up
                return ENUM_Direction.Up;
            }

            /// <summary>
            /// Responsible for changing the position of the agent
            /// </summary>
            /// <param name="map"> Gets the information of the map </param>
            /// <param name="direction"> Gets the intended direction of the agent </param>
            /// <returns></returns>
            protected bool MoveToDirection(Map map, ENUM_Direction direction)
            {
                // Declares to new ints that save current position on X and Y
                int newX = Position.X;
                int newY = Position.Y;

                // Switch case to give the ints NewX and NewY the new value
                switch (direction)
                {
                    // If the direction is up
                    case ENUM_Direction.Up:
                        // If the current position on Y is 0 (is on the edge)
                        if (Position.Y == 0)
                            // The newY variable is the max height value of the map -1
                            newY = map.pieces.GetLength(1) - 1;
                        else
                            // The newY value is the current Y position -1
                            newY = Position.Y - 1;
                        break;

                    // If the direction is down
                    case ENUM_Direction.Down:
                        // If the current position on Y is on the bottom edge
                        if (Position.Y == map.pieces.GetLength(1) - 1)
                            // The newY is 0
                            newY = 0;
                        else
                            // The newY value is the current Y position -1
                            newY = Position.Y + 1;
                        break;

                    // If the direction is left
                    case ENUM_Direction.Left:
                        // If the current position on X is 0 (is on the edge)
                        if (Position.X == 0)
                            // The newX variable is the max width value of the map -1
                            newX = map.pieces.GetLength(0) - 1;
                        else
                            // The newX value is the current X position -1
                            newX = Position.X - 1;
                        break;

                    // If the direction is right
                    case ENUM_Direction.Right:
                        // If the current position on X is on the bottom edge
                        if (Position.X == map.pieces.GetLength(0) - 1)
                            // the newX is 0
                            newX = 0;
                        else
                            // The newX value is the current X position -1
                            newX = Position.X + 1;
                        break;
                }

                // If that position is not empty space return false
                if (!(map.pieces[newX, newY] is EmptySpace))
                    return false;

                // Creates a new vector 2 that saves the old position
                Vector2 oldPosition = new Vector2(Position.X, Position.Y);

                // Transforms the current position into the new one
                Position = new Vector2(newX, newY);

                //  calls OnMoved event with given parameters if it's not null
                OnMoved?.Invoke(this, direction, oldPosition);

                // Returns true
                return true;
            }

            public abstract void Move(Map map);

            /// <summary>
            /// Goes to MoveToDirection and returns true or false based on the function
            /// </summary>
            /// <param name="map"> Takes in the information of the map </param>
            /// <param name="direction"> Gets the desired direction </param>
            /// <returns></returns>
            public bool Move(Map map, ENUM_Direction direction)
            {
                // Returns a bool depending on the function MoveToDirection
                return MoveToDirection(map, direction);
            }
        }
    }
}
