using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public static class Application
    {
        static Map map;

        public static bool GetUInt(string text, ref uint value)
        {
            return uint.TryParse(text, out value);
        }

        public static void Initialize()
        {
            uint mapWidth = 0;
            Console.WriteLine("Map Width : ");
            while (!GetUInt(Console.ReadLine(), ref mapWidth)) ;

            uint mapHeight = 0;
            Console.WriteLine("Map Height : ");
            while (!GetUInt(Console.ReadLine(), ref mapHeight)) ;

            uint zombieCount = 0;
            Console.WriteLine("Zombie Count : ");
            while (!GetUInt(Console.ReadLine(), ref zombieCount) && zombieCount >= (mapWidth * mapHeight) - 1) ;

            uint playerCount = 0;
            Console.WriteLine("Player Count : ");
            while (!GetUInt(Console.ReadLine(), ref playerCount) && playerCount + zombieCount >= (mapWidth * mapHeight) - 1) ;

            uint playerControlCount = 0;
            Console.WriteLine("How many players will you control : ");
            while (!GetUInt(Console.ReadLine(), ref playerControlCount) && playerControlCount > playerCount) ;

            map = new Map(mapWidth, mapHeight, playerCount, playerControlCount, zombieCount);
        }

        public static void Run()
        {
            bool quit = false;

            do
            {
                Console.Clear();

                map.Simulate();
                string s = Console.ReadLine();
                quit = s == "y";


            }
            while (!quit);
        }
    }
}
