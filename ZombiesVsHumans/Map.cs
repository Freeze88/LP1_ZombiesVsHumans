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
            char prefix = ' ';

            public MapPiece(char prefix = ' ', ConsoleColor color = ConsoleColor.White)
            {
                this.prefix = prefix;
                this.color = color;
                Hash = float.MaxValue;
            }

            public override string ToString()
            {
                return prefix.ToString();
            }

            public void SetHash(float value)
            {
                Hash = value;
            }

            public virtual void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                Console.ForegroundColor = this.color;
                Console.Write(prefix);

                Console.ForegroundColor = color;
            }

            public float Hash { get; private set; }
        }


        MapPiece[,] pieces;

        public Map(uint mapWidth, uint mapHeight, uint playerCount, uint playersToControl, uint zombieCount)
        {
            pieces = new MapPiece[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
                for (int x = 0; x < mapWidth; x++)
                    pieces[x, y] = new EmptySpace();

            for (uint i = 0, j = 0; i < playerCount; i++, j++)
            {
                pieces[Mathf.RandomRange(1, (int)mapWidth), Mathf.RandomRange(1, (int)mapHeight)] = new Player(j <= playersToControl);
            }

            for (uint i = 0; i < zombieCount; i++)
                pieces[Mathf.RandomRange(1, (int)mapWidth), Mathf.RandomRange(1, (int)mapHeight)] = new Zombie();

            CalculateHash();
        }

        public void CalculateHash()
        {
            for (uint y1 = 0; y1 < pieces.GetLength(1); y1++)
                for (uint x1 = 0; x1 < pieces.GetLength(0); x1++)
                {
                    if (pieces[x1, y1] is Player)
                    {
                        uint[] playerPos = new uint[] { x1, y1 };

                        for (uint y = 0; y < pieces.GetLength(1); y++)
                            for (uint x = 0; x < pieces.GetLength(0); x++)
                            {
                                float yPos = y;
                                if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x, y - pieces.GetLength(1), playerPos[0], playerPos[1]))
                                    yPos = y - pieces.GetLength(1);

                                float xPos = x;
                                if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x - pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                    xPos = x - pieces.GetLength(0);

                                float pxPos = playerPos[0];
                                float pyPos = playerPos[1];

                                float min = (float)Math.Sqrt(Math.Pow(pxPos - xPos, 2) + Math.Pow(pyPos - yPos, 2));
                                pieces[x, y].SetHash(Mathf.Min(min, pieces[x, y].Hash));
                            }
                    }
                }
        }
        public float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public void Simulate()
        {
            for (uint y = 0; y < pieces.GetLength(1); y++)
            {
                for (uint x = 0; x < pieces.GetLength(0); x++)
                {
                    pieces[x, y].Print();
                }
                Console.WriteLine();
            }
        }
    }
}
