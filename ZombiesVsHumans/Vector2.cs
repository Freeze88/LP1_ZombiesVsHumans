using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    internal sealed class Vector2
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }

        public static float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
    }
}