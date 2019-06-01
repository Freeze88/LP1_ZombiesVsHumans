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
            public Zombie(Map map)
                : base(map, 'z', ConsoleColor.Red)
            {

            }

            private ENUM_Direction GetDirectionFromLowestHash(uint x, uint y)
            {
                MapPiece rPiece = null;
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, (int)x, (int)y - 1);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, (int)x - 1, (int)y);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, (int)x + 1, (int)y);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, (int)x, (int)y + 1);

                rPiece = topPiece.PlayerHash < bottomPiece.PlayerHash ? topPiece : bottomPiece;
                rPiece = rPiece.PlayerHash < leftPiece.PlayerHash ? rPiece : leftPiece;
                rPiece = rPiece.PlayerHash < rightPiece.PlayerHash ? rPiece : rightPiece;

                if (rPiece == bottomPiece)
                    return ENUM_Direction.Down;
                else if (rPiece == leftPiece)
                    return ENUM_Direction.Left;
                else if (rPiece == rightPiece)
                    return ENUM_Direction.Right;

                return ENUM_Direction.Up;
            }

            public override void Move()
            {
                for (uint y = 0; y < map.pieces.GetLength(1); y++)
                    for (uint x = 0; x < map.pieces.GetLength(0); x++)
                        if (map.pieces[x, y] == this)
                        {
                            ENUM_Direction direction = GetDirectionFromLowestHash(x, y);

                            uint newX = x;
                            uint newY = y;

                            switch (direction)
                            {
                                case ENUM_Direction.Up:
                                    if (y == 0)
                                        newY = (uint)map.pieces.GetLength(1) - 1;
                                    else
                                        newY = y - 1;
                                    break;
                                case ENUM_Direction.Down:
                                    if (y == map.pieces.GetLength(1) - 1)
                                        newY = 0;
                                    else
                                        newY = y + 1;
                                    break;
                                case ENUM_Direction.Left:
                                    if (x == 0)
                                        newX = (uint)map.pieces.GetLength(0) - 1;
                                    else
                                        newX = x - 1;
                                    break;
                                case ENUM_Direction.Right:
                                    if (x == map.pieces.GetLength(0) - 1)
                                        newX = 0;
                                    else
                                        newX = x + 1;
                                    break;
                            }

                            Console.WriteLine("Moved " + direction.ToString());

                            EmptySpace space = new EmptySpace(map);
                            space.SetPlayerHash(map.pieces[x, y].PlayerHash);
                            space.SetZombieHash(map.pieces[x, y].ZombieHash);
                            map.pieces[x, y] = space;

                            this.SetZombieHash(map.pieces[newX, newY].ZombieHash);
                            this.SetPlayerHash(map.pieces[newX, newY].PlayerHash);

                            map.pieces[newX, newY] = this;

                            return;
                        }
            }
        }
    }
}
