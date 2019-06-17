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

            private ENUM_Direction GetDirectionFromLowestHash(Map map, Vector2 position)
            {
                MapPiece rPiece = null;
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, position);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, position);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, position);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, position);

                rPiece = topPiece;
                rPiece = rPiece.PlayerHash < bottomPiece.PlayerHash ? rPiece : bottomPiece;
                rPiece = rPiece.PlayerHash < leftPiece.PlayerHash ? rPiece : leftPiece;
                rPiece = rPiece.PlayerHash < rightPiece.PlayerHash ? rPiece : rightPiece;

                if (rPiece == bottomPiece && bottomPiece is EmptySpace)
                    return ENUM_Direction.Down;
                else if (rPiece == leftPiece && leftPiece is EmptySpace)
                    return ENUM_Direction.Left;
                else if (rPiece == rightPiece && rightPiece is EmptySpace)
                    return ENUM_Direction.Right;

                return ENUM_Direction.Up;
            }

            public static void CalculateHash(Map map)
            {
                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                        map.pieces[x1, y1].SetZombieHash(float.MaxValue);

                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                    {
                        if (map.pieces[x1, y1] is Zombie)
                        {
                            uint[] playerPos = new uint[] { x1, y1 };

                            for (uint y = 0; y < map.pieces.GetLength(1); y++)
                                for (uint x = 0; x < map.pieces.GetLength(0); x++)
                                {
                                    float yPos = (float)y;
                                    if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x, y - map.pieces.GetLength(1), playerPos[0], playerPos[1]))
                                        yPos = y - map.pieces.GetLength(1);
                                    else if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x, y + map.pieces.GetLength(1), playerPos[0], playerPos[1]))
                                        yPos = Math.Abs(y + map.pieces.GetLength(1));

                                    float xPos = x;
                                    if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x - map.pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                        xPos = x - map.pieces.GetLength(0);
                                    else if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x + map.pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                        xPos = x + map.pieces.GetLength(0);

                                    float pxPos = (float)playerPos[0];
                                    float pyPos = (float)playerPos[1];

                                    float min = (float)Math.Sqrt(Math.Pow(pxPos - xPos, 2) + Math.Pow(pyPos - yPos, 2));
                                    map.pieces[x, y].SetZombieHash(Mathf.Min(min, map.pieces[x, y].ZombieHash));
                                }
                        }
                    }
            }

            public static void Infect(Map map)
            {
                foreach (Zombie zombie in map.zombies.ToArray())
                {
                    MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, zombie.Position);
                    MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, zombie.Position);
                    MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, zombie.Position);
                    MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, zombie.Position);

                    if (topPiece is Player playa1)
                    {
                        map.Add(new Zombie(new Vector2(topPiece.Position)));

                        playa1.Infect();
                    }

                    if (bottomPiece is Player playa2)
                    {
                        map.Add(new Zombie(new Vector2(bottomPiece.Position)));

                        playa2.Infect();
                    }

                    if (rightPiece is Player playa3)
                    {
                        map.Add(new Zombie(new Vector2(rightPiece.Position)));

                        playa3.Infect();
                    }

                    if (leftPiece is Player playa4)
                    {
                        map.Add(new Zombie(new Vector2(leftPiece.Position)));

                        playa4.Infect();
                    }
                }
            }

            public override void Move(Map map)
            {
                ENUM_Direction direction = GetDirectionFromLowestHash(map, Position);

                MoveToDirection(map, direction);
            }
        }
    }
}
