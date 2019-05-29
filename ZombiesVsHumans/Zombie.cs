using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal partial class Zombie : MapPiece
        {
            public Zombie() : base('z', ConsoleColor.Red) { }
        }
    }
}
