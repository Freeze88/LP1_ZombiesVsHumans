using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal abstract class Character : MapPiece
        {
            public delegate void MoveEventHandler(Character sender, ENUM_Direction direction, Vector2 oldPosition);

            public event MoveEventHandler OnMoved;

            public Character(Vector2 position, char prefix, ConsoleColor color = ConsoleColor.DarkBlue)
                : base(position, prefix, color)
            {

            }

            protected ENUM_Direction GetDirectionFromHash(Map map, Vector2 position, bool isZombie)
            {
                MapPiece rPiece = null;
                MapPiece topPiece = map.GetPiece(ENUM_Direction.Up, position);
                MapPiece leftPiece = map.GetPiece(ENUM_Direction.Left, position);
                MapPiece rightPiece = map.GetPiece(ENUM_Direction.Right, position);
                MapPiece bottomPiece = map.GetPiece(ENUM_Direction.Down, position);

                rPiece = topPiece;
                if (isZombie)
                {
                    rPiece = rPiece.PlayerHash < bottomPiece.PlayerHash ? rPiece : bottomPiece;
                    rPiece = rPiece.PlayerHash < leftPiece.PlayerHash ? rPiece : leftPiece;
                    rPiece = rPiece.PlayerHash < rightPiece.PlayerHash ? rPiece : rightPiece;
                }
                else
                {
                    rPiece = rPiece.ZombieHash > bottomPiece.ZombieHash ? rPiece : bottomPiece;
                    rPiece = rPiece.ZombieHash > leftPiece.ZombieHash ? rPiece : leftPiece;
                    rPiece = rPiece.ZombieHash > rightPiece.ZombieHash ? rPiece : rightPiece;
                }

                if (rPiece == bottomPiece)
                    return ENUM_Direction.Down;
                else if (rPiece == leftPiece)
                    return ENUM_Direction.Left;
                else if (rPiece == rightPiece)
                    return ENUM_Direction.Right;

                return ENUM_Direction.Up;
            }

            protected bool MoveToDirection(Map map, ENUM_Direction direction)
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

                OnMoved?.Invoke(this, direction, oldPosition);

                return true;
            }

            public abstract void Move(Map map);

            public bool Move(Map map, ENUM_Direction direction)
            {
                return MoveToDirection(map, direction);
            }
        }
    }
}
