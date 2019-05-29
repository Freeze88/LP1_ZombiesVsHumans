using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public static class Mathf
    {
        static Random rand = new Random();

        public static int Min(int value1, int value2)
        {
            if (value1 < value2)
                return value1;

            return value2;
        }

        public static float Min(float value1, float value2)
        {
            if (value1 < value2)
                return value1;

            return value2;
        }

        public static int RandomRange(int min, int max)
        {

            if (min > max)
            {
                int aux = max;
                max = min;
                min = aux;
            }

            return rand.Next(min, max);
        }
    }
}
