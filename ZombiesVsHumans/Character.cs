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

            protected void MoveToDirection(Map map, ENUM_Direction direction)
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
                    return;

                Vector2 oldPosition = new Vector2(Position.X, Position.Y);

                Position = new Vector2(newX, newY);

                HasMoved(direction, oldPosition);
            }

            protected void HasMoved(ENUM_Direction direction, Vector2 oldPosition)
            {
                OnMoved?.Invoke(this, direction, oldPosition);
            }

            public abstract void Move(Map map);

            public virtual bool Move(Map map, ENUM_Direction direction)
            {
                return false;
            }
        }
    }
}
