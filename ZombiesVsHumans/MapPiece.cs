using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal abstract class MapPiece
        {
            protected ConsoleColor color;
            protected char prefix = ' ';

            public MapPiece(Vector2 position, char prefix = ' ', ConsoleColor color = ConsoleColor.White)
            {
                Position = position;
                this.prefix = prefix;
                this.color = color;
                PlayerHash = float.MaxValue;
                ZombieHash = float.MaxValue;
            }

            public override string ToString() => prefix.ToString();

            public void SetPlayerHash(float value) => PlayerHash = value;

            public void SetZombieHash(float value) => ZombieHash = value;

            public virtual void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                Console.ForegroundColor = this.color;
                Console.Write("  " + prefix + "  ");

                Console.ForegroundColor = color;
            }

            public float PlayerHash { get; private set; }
            public float ZombieHash { get; private set; }
            public Vector2 Position { get; protected set; }
        }
    }
}
