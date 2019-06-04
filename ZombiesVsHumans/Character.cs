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
