using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal partial class Zombie : Character
        {
            public Zombie(Vector2 position)
                : base(position, 'z', ConsoleColor.Red)
            {

            }

            public void Propegate(Map map)
            {
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, Position);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, Position);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, Position);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, Position);

                if (topPiece is Player playa1)
                {
                    playa1.Infect();
                    map.Add(new Zombie(new Vector2(topPiece.Position)));
                }

                if (bottomPiece is Player playa2)
                {
                    playa2.Infect();
                    map.Add(new Zombie(new Vector2(bottomPiece.Position)));
                }

                if (rightPiece is Player playa3)
                {
                    playa3.Infect();
                    map.Add(new Zombie(new Vector2(rightPiece.Position)));
                }

                if (leftPiece is Player playa4)
                {
                    playa4.Infect();
                    map.Add(new Zombie(new Vector2(leftPiece.Position)));
                }
            }

            public static void Infect(Map map)
            {
                foreach (Zombie zombie in map.zombies.ToArray())
                    zombie.Propegate(map);
            }

            public override void Move(Map map)
            {
                ENUM_Direction direction = GetDirectionFromHash(map, Position, true);

                MoveToDirection(map, direction);
            }
        }
    }
}
