using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal partial class Player : Character
        {
            public delegate void EventHandler(Player sender);

            public event EventHandler OnInfected;

            public Player(Vector2 position, bool canControl, bool isTurn = false)
                : base(position, 'p', ConsoleColor.Blue)
            {
                CanControl = canControl;
                IsTurn = isTurn;
            }

            public void SetTurn(bool value) => IsTurn = value;

            public void Infect() => OnInfected?.Invoke(this);

            public override void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                if (IsTurn && CanControl)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("  P  ");
                }
                else if (CanControl)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("  P  ");
                }
                else
                {
                    Console.ForegroundColor = this.color;
                    Console.Write("  H  ");

                }
                Console.ForegroundColor = color;
            }

            public override void Move(Map map)
            {
                ENUM_Direction direction = GetDirectionFromHash(map, Position, false);

                MoveToDirection(map, direction);
            }

            public bool CanControl { get; private set; }
            public bool IsTurn { get; private set; }
        }
    }
}
