using System;

namespace ZombiesVsHumans
{
    internal sealed class Vector2
    {
        // Returns an int X and sets it privatly
        public int X { get; private set; }
        // Returns an int Y and sets it privatly
        public int Y { get; private set; }

        /// <summary>
        /// Constructor of the Vector2 class
        /// </summary>
        /// <param name="x"> The X coordinate of the vector2 </param>
        /// <param name="y"> The Y coordinate of the vector2 </param>
        public Vector2(int x, int y)
        {
            // Sets this class X to the X given
            X = x;
            // Sets this class Y to the Y given
            Y = y;
        }

        /// <summary>
        /// Takes in a vector2 and replaces its values for the ones given
        /// </summary>
        /// <param name="vector"> A vector2 prvidided </param>
        public Vector2(Vector2 vector)
        {
            // Sets this class X to the X of the vector given
            X = vector.X;
            // Sets this class Y to the Y of the vector given
            Y = vector.Y;
        }

        /// <summary>
        /// Measures the distance between two values
        /// </summary>
        /// <param name="x1"> The first X value given </param>
        /// <param name="y1"> The first Y value given </param>
        /// <param name="x2"> The second X value given </param>
        /// <param name="y2"> The second Y value given </param>
        /// <returns> The distance between the two points </returns>
        public static float Distance(float x1, float y1, float x2, float y2)
        {
            // Returns the distance between the two points
            // square root of (((x2-x1)^2) + ((y2-y1)^2))
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}
