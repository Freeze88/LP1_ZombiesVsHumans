using System;
using System.Collections.Generic;
using System.Text;

namespace ZombiesVsHumans
{
    class EmptySpace : Map.MapPiece
    {
        public EmptySpace(Map map) : base(map,' ', ConsoleColor.White) { }

        public override void Print()
        {
            ConsoleColor color = Console.ForegroundColor;

            Console.ForegroundColor = this.color;
            Console.Write(" . ");

            Console.ForegroundColor = color;
        }
    }
}
