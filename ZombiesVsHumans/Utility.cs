using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public static class Utility
    {
        public static bool GetUInt(string text, ref uint value)
        {
            return uint.TryParse(text, out value);
        }

        public static Map.ENUM_Direction GetInverseDirection(Map.ENUM_Direction direction)
        {
            switch (direction)
            {
                case Map.ENUM_Direction.Up:
                    return Map.ENUM_Direction.Down;
                case Map.ENUM_Direction.Down:
                    return Map.ENUM_Direction.Up;
                case Map.ENUM_Direction.Left:
                    return Map.ENUM_Direction.Right;
                case Map.ENUM_Direction.Right:
                    return Map.ENUM_Direction.Left;
            }

            return Map.ENUM_Direction.Up;
        }
    }
}
