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
    }
}
