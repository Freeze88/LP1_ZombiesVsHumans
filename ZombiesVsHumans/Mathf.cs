using System;

namespace ZombiesVsHumans
{
    /// <summary>
    /// A class to aid the mathematical operations of the program
    /// </summary>
    public static class Mathf
    {
        // Creates a random number called rand
        private static Random rand = new Random();

        /// <summary>
        /// Gets the lowest value of the two given
        /// </summary>
        /// <param name="value1"> The first value provided </param>
        /// <param name="value2"> The second value provided </param>
        /// <returns></returns>
        public static int Min(int value1, int value2)
        {
            // Checks if the first value is smaller than the second
            if (value1 < value2)
                // If it is returns the first value
                return value1;

            // Else returns the second value
            return value2;
        }

        /// <summary>
        /// Gets the lowest value of the two given
        /// </summary>
        /// <param name="value1"> The first value provided </param>
        /// <param name="value2"> The second value provided </param>
        /// <returns></returns>
        public static float Min(float value1, float value2)
        {
            // Checks if the first value is smaller than the second
            if (value1 < value2)
                // If it is returns the first value
                return value1;

            // Else returns the second value
            return value2;
        }

        /// <summary>
        /// Returns a random number between the two values given
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomRange(int min, int max)
        {
            // Checks if the minimun value is bigger than the max value
            if (min > max)
            {
                // Creates an auxaliary int aux and gives it the max value
                int aux = max;
                // Switches the minimun value with the max
                max = min;
                // gives the minimum value the max value
                min = aux;
            }

            // Returns a number between the two
            return rand.Next(min, max);
        }
    }
}
