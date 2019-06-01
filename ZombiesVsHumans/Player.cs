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
            public bool CanControl { get; private set; }
            public bool IsTurn { get; private set; }
            public void SetTurn(bool value) => IsTurn = value;

            public Player(Map map, bool canControl, bool isTurn = false)
                : base(map, 'p', ConsoleColor.Blue)
            {
                CanControl = canControl;
                IsTurn = IsTurn;
            }

            public override void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                if (CanControl)
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                else if (IsTurn)
                    Console.ForegroundColor = ConsoleColor.Magenta;
                else
                    Console.ForegroundColor = this.color;

                Console.Write(" " + prefix + " ");

                Console.ForegroundColor = color;
            }

            private ENUM_Direction GetDirectionFromLowestHash(uint x, uint y)
            {
                MapPiece rPiece = null;
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, (int)x, (int)y);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, (int)x, (int)y);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, (int)x, (int)y);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, (int)x, (int)y);

                rPiece = topPiece.ZombieHash < bottomPiece.ZombieHash ? topPiece : bottomPiece;
                rPiece = rPiece.ZombieHash < leftPiece.ZombieHash ? rPiece : leftPiece;
                rPiece = rPiece.ZombieHash < rightPiece.ZombieHash ? rPiece : rightPiece;

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
                                        newY = x - 1;
                                    break;
                                case ENUM_Direction.Right:
                                    if (x == map.pieces.GetLength(0) - 1)
                                        newX = 0;
                                    else
                                        newX = x + 1;
                                    break;
                            }

                            map.pieces[x, y] = new EmptySpace(map);
                            map.pieces[newX, newY] = this;

                            return;
                        }
            }
        }
    }
}
