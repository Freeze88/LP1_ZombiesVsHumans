using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        internal abstract class MapPiece
        {
            protected ConsoleColor color;
            protected char prefix = ' ';

            public MapPiece(Vector2 position, char prefix = ' ', ConsoleColor color = ConsoleColor.White)
            {
                Position = position;
                this.prefix = prefix;
                this.color = color;
                PlayerHash = float.MaxValue;
                ZombieHash = float.MaxValue;
            }

            public override string ToString() => prefix.ToString();

            public void SetPlayerHash(float value) => PlayerHash = value;

            public void SetZombieHash(float value) => ZombieHash = value;

            public static void CalculateHash(Map map)
            {
                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                        map.pieces[x1, y1].SetPlayerHash(float.MaxValue);

                for (uint y1 = 0; y1 < map.pieces.GetLength(1); y1++)
                    for (uint x1 = 0; x1 < map.pieces.GetLength(0); x1++)
                    {
                        bool isZombie = map.pieces[x1, y1] is Zombie;
                        bool isPlayer = map.pieces[x1, y1] is Player;

                        if (isZombie || isPlayer)
                        {
                            uint[] playerPos = new uint[] { x1, y1 };

                            for (uint y = 0; y < map.pieces.GetLength(1); y++)
                                for (uint x = 0; x < map.pieces.GetLength(0); x++)
                                {
                                    float yPos = (float)y;
                                    if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x, y - map.pieces.GetLength(1), playerPos[0], playerPos[1]))
                                        yPos = y - map.pieces.GetLength(1);
                                    else if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x, y + map.pieces.GetLength(1), playerPos[0], playerPos[1]))
                                        yPos = Math.Abs(y + map.pieces.GetLength(1));

                                    float xPos = x;
                                    if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x - map.pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                        xPos = x - map.pieces.GetLength(0);
                                    else if (Vector2.Distance(x, y, playerPos[0], playerPos[1]) > Vector2.Distance(x + map.pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                        xPos = x + map.pieces.GetLength(0);

                                    float pxPos = (float)playerPos[0];
                                    float pyPos = (float)playerPos[1];

                                    float min = (float)Math.Sqrt(Math.Pow(pxPos - xPos, 2) + Math.Pow(pyPos - yPos, 2));

                                    if (isPlayer)
                                        map.pieces[x, y].SetPlayerHash(Mathf.Min(min, map.pieces[x, y].PlayerHash));

                                    if (isZombie)
                                        map.pieces[x, y].SetZombieHash(Mathf.Min(min, map.pieces[x, y].ZombieHash));
                                }
                        }
                    }
            }

            public virtual void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                Console.ForegroundColor = this.color;
                Console.Write("  " + prefix + "  ");

                Console.ForegroundColor = color;
            }

            public float PlayerHash { get; private set; }
            public float ZombieHash { get; private set; }
            public Vector2 Position { get; protected set; }
        }
    }
}

