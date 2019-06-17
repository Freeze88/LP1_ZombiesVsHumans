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

            private ENUM_Direction GetDirectionFromHighestHash(Map map, Vector2 position)
            {
                MapPiece rPiece = null;
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, position);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, position);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, position);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, position);

                rPiece = topPiece;
                rPiece = rPiece.ZombieHash > bottomPiece.ZombieHash ? rPiece : bottomPiece;
                rPiece = rPiece.ZombieHash > leftPiece.ZombieHash ? rPiece : leftPiece;
                rPiece = rPiece.ZombieHash > rightPiece.ZombieHash ? rPiece : rightPiece;

                if (rPiece == bottomPiece && bottomPiece is EmptySpace)
                    return ENUM_Direction.Down;
                else if (rPiece == leftPiece && leftPiece is EmptySpace)
                    return ENUM_Direction.Left;
                else if (rPiece == rightPiece && rightPiece is EmptySpace)
                    return ENUM_Direction.Right;

                return ENUM_Direction.Up;
            }

            public override void Move(Map map)
            {
                ENUM_Direction direction = GetDirectionFromHighestHash(map, Position);

                MoveToDirection(map, direction);
            }

            public override bool Move(Map map, ENUM_Direction direction)
            {
                int newX = Position.X;
                int newY = Position.Y;

                switch (direction)
                {
                    case ENUM_Direction.Up:
                        if (Position.Y == 0)
                            newY = map.pieces.GetLength(1) - 1;
                        else
                            newY = Position.Y - 1;
                        break;
                    case ENUM_Direction.Down:
                        if (Position.Y == map.pieces.GetLength(1) - 1)
                            newY = 0;
                        else
                            newY = Position.Y + 1;
                        break;
                    case ENUM_Direction.Left:
                        if (Position.X == 0)
                            newX = map.pieces.GetLength(0) - 1;
                        else
                            newX = Position.X - 1;
                        break;
                    case ENUM_Direction.Right:
                        if (Position.X == map.pieces.GetLength(0) - 1)
                            newX = 0;
                        else
                            newX = Position.X + 1;
                        break;
                }

                if (!(map.pieces[newX, newY] is EmptySpace))
                    return false;

                Vector2 oldPosition = new Vector2(Position.X, Position.Y);

                Position = new Vector2(newX, newY);

                HasMoved(direction, oldPosition);

                return true;
            }

            public static void CalculateHash(Map map)
            {
                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                        map.pieces[x1, y1].SetPlayerHash(float.MaxValue);

                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                    {
                        if (map.pieces[x1, y1] is Player)
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
                                    map.pieces[x, y].SetPlayerHash(Mathf.Min(min, map.pieces[x, y].PlayerHash));
                                }
                        }
                    }
            }

            public bool CanControl { get; private set; }
            public bool IsTurn { get; private set; }
        }
    }
}
