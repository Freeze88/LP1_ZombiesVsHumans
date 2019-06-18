using System;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal partial class Human : Agents
        {
            public delegate void EventHandler(Human sender);

            public event EventHandler OnInfected;

            /// <summary>
            /// Constructor of the Human class
            /// </summary>
            /// <param name="position"> The desired position </param>
            /// <param name="canControl"> If the human is controllable by the player </param>
            /// <param name="isTurn"> If it's its turn to move </param>
            public Human(Vector2 position, bool canControl, bool isTurn = false)
                : base(position, 'p', ConsoleColor.Blue)
            {
                // Initializes this CanControl as the one provided
                CanControl = canControl;
                // And the IsTurn to the one provided
                IsTurn = isTurn;
            }

            /// <summary>
            /// Changes the turn outside this class
            /// </summary>
            /// <param name="value"> A bool given from outside </param>
            public void SetTurn(bool value) => IsTurn = value;

            /// <summary>
            /// Starts the event OnInfected that calls OnPlayerInfected
            /// </summary>
            public void Infect() => OnInfected?.Invoke(this);

            /// <summary>
            /// Responsible for showing on the console the humans
            /// </summary>
            public override void Print()
            {
                // Sets the corrent console color to the variable color
                ConsoleColor color = Console.ForegroundColor;

                // if it's the Players turn
                if (IsTurn && CanControl)
                {
                    // Sets the font color to magenta
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    // Displays a "P"
                    Console.Write("  P  ");
                }
                // If it's not the players turn
                else if (CanControl)
                {
                    // Sets the font color to green
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    // Displays a "P"
                    Console.Write("  P  ");
                }
                // If it's an NPC falls on the else
                else
                {
                    // Sets the color to the color of the class
                    Console.ForegroundColor = this.color;
                    // Displays a "H"
                    Console.Write("  H  ");

                }
                // Reverts the color to its original one
                Console.ForegroundColor = color;
            }

            /// <summary>
            /// Moves the human to the desired position
            /// </summary>
            /// <param name="map"> Gets the current state of the map </param>
            public override void Move(Map map)
            {
                // Gives the enum direction the value given by GetDirectionFromHash of the character class
                ENUM_Direction direction = GetDirectionFromHash(map, Position, false);

                // Goes to MoveToDirection taking the new direction and the map
                MoveToDirection(map, direction);
            }

            // Gets the canControl bool of the human
            public bool CanControl { get; private set; }
            // Gets the IsTurn bool of the human
            public bool IsTurn { get; private set; }
        }
    }
}
