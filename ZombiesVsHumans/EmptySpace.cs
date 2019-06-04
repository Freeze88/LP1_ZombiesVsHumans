using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal class EmptySpace : MapPiece
        {
            public EmptySpace(Vector2 position)
                : base(position, ' ', ConsoleColor.White)
            {

            }

            public override void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                Console.ForegroundColor = this.color;
                //Console.Write(string.Format(" {0:F2} ", ZombieHash));
                Console.Write("  .  ");

                Console.ForegroundColor = color;
            }
        }
    }
}
