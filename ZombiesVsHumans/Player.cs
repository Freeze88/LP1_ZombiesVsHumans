using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal partial class Player : MapPiece
        {
            public Player(bool canControl) : base('p', ConsoleColor.Blue)
            {
                CanControl = canControl;
            }

            public override void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                Console.ForegroundColor = this.color;
                Console.Write(" P ;");

                Console.ForegroundColor = color;
            }

            public bool CanControl { get; private set; }
        }
    }
}
