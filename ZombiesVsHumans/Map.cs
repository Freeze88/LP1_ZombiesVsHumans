using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombiesVsHumans
{
    public partial class Map
    {
        public enum ENUM_Direction : ushort
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        internal abstract class MapPiece
        {
            protected ConsoleColor color;
            protected char prefix = ' ';
            protected readonly Map map;

            public MapPiece(Map map, char prefix = ' ', ConsoleColor color = ConsoleColor.White)
            {
                this.map = map;
                this.prefix = prefix;
                this.color = color;
                PlayerHash = float.MaxValue;
                ZombieHash = float.MaxValue;
            }

            public override string ToString() => prefix.ToString();

            public void SetPlayerHash(float value) => PlayerHash = value;

            public void SetZombieHash(float value) => ZombieHash = value;

            public virtual void Move()
            {

            }

            public virtual void Print()
            {
                ConsoleColor color = Console.ForegroundColor;

                Console.ForegroundColor = this.color;
                Console.Write(" " + prefix + " ");

                Console.ForegroundColor = color;
            }

            public float PlayerHash { get; private set; }
            public float ZombieHash { get; private set; }
        }

        MapPiece[,] pieces;
        List<Player> players = new List<Player>();
        List<Player> NPCs = new List<Player>();
        List<Zombie> zombies = new List<Zombie>();
        int playerIndex = 0;

        public Map(uint mapWidth, uint mapHeight, uint playerCount, uint playersToControl, uint zombieCount)
        {
            pieces = new MapPiece[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
                for (int x = 0; x < mapWidth; x++)
                    pieces[x, y] = new EmptySpace(this);

            for (uint i = 0, j = 0; i < playerCount; i++, j++)
                PlacePlayer(j < playersToControl, i == 0);

            //pieces[mapWidth - 1, mapHeight - 1] = new Player(this, false, false);

            for (uint i = 0; i < zombieCount; i++)
                PlaceZombie();

            CalculatePlayerHash();
            CalculateZombieHash();
        }

        private void PlacePlayer(bool canControl, bool isTurn)
        {
            Player playa = new Player(this, canControl, isTurn);

            if (canControl)
                players.Add(playa);
            else
                NPCs.Add(playa);

            pieces[Mathf.RandomRange(0, pieces.GetLength(0)), Mathf.RandomRange(0, pieces.GetLength(1))] = playa;
        }

        private void PlaceZombie()
        {
            int x = 0,
                y = 0;
            do
            {
                x = Mathf.RandomRange(1, (int)pieces.GetLength(0));
                y = Mathf.RandomRange(1, (int)pieces.GetLength(1));
            } while (!(pieces[x, y] is EmptySpace));

            Zombie zombie = new Zombie(this);

            zombies.Add(zombie);

            pieces[x, y] = zombie;
        }

        public float Distance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public void CalculatePlayerHash()
        {
            for (uint y1 = 0; y1 < pieces.GetLength(1); y1++)
                for (uint x1 = 0; x1 < pieces.GetLength(0); x1++)
                    pieces[x1, y1].SetPlayerHash(float.MaxValue);

            for (uint y1 = 0; y1 < pieces.GetLength(1); y1++)
                for (uint x1 = 0; x1 < pieces.GetLength(0); x1++)
                {
                    if (pieces[x1, y1] is Player)
                    {
                        uint[] playerPos = new uint[] { x1, y1 };

                        for (uint y = 0; y < pieces.GetLength(1); y++)
                            for (uint x = 0; x < pieces.GetLength(0); x++)
                            {
                                float yPos = (float)y;
                                if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x, y - pieces.GetLength(1), playerPos[0], playerPos[1]))
                                    yPos = y - pieces.GetLength(1);
                                else if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x, playerPos[1] - y - 1, playerPos[0], playerPos[1]))
                                    yPos = playerPos[1] - y - 1;

                                float xPos = x;
                                if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x - pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                    xPos = x - pieces.GetLength(0);
                                else if (Distance(x, y, playerPos[0], playerPos[1]) > Distance((playerPos[0] - x - 1), y, playerPos[0], playerPos[1]))
                                    xPos = playerPos[0] - x - 1;

                                float pxPos = (float)playerPos[0];
                                float pyPos = (float)playerPos[1];

                                float min = (float)Math.Sqrt(Math.Pow(pxPos - xPos, 2) + Math.Pow(pyPos - yPos, 2));
                                pieces[x, y].SetPlayerHash(Mathf.Min(min, pieces[x, y].PlayerHash));
                            }
                    }
                }
        }

        public void CalculateZombieHash()
        {
            for (uint y1 = 0; y1 < pieces.GetLength(1); y1++)
                for (uint x1 = 0; x1 < pieces.GetLength(0); x1++)
                    pieces[x1, y1].SetZombieHash(float.MaxValue);

            for (uint y1 = 0; y1 < pieces.GetLength(1); y1++)
                for (uint x1 = 0; x1 < pieces.GetLength(0); x1++)
                {
                    if (pieces[x1, y1] is Zombie)
                    {
                        uint[] playerPos = new uint[] { x1, y1 };

                        for (uint y = 0; y < pieces.GetLength(1); y++)
                            for (uint x = 0; x < pieces.GetLength(0); x++)
                            {
                                float yPos = (float)y;
                                if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x, y - pieces.GetLength(1), playerPos[0], playerPos[1]))
                                    yPos = y - pieces.GetLength(1);
                                else if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(x, playerPos[1] - y - 1, playerPos[0], playerPos[1]))
                                    yPos = playerPos[1] - y - 1;

                                float xPos = x;
                                if (Distance(x, y, playerPos[0], playerPos[1]) > Distance(Math.Abs(x - pieces.GetLength(0)) % pieces.GetLength(0), y, playerPos[0], playerPos[1]))
                                    xPos = x - pieces.GetLength(0);
                                else if (Distance(x, y, playerPos[0], playerPos[1]) > Distance((playerPos[0] - x - 1), y, playerPos[0], playerPos[1]))
                                    xPos = playerPos[0] - x - 1;

                                float pxPos = (float)playerPos[0];
                                float pyPos = (float)playerPos[1];

                                float min = (float)Math.Sqrt(Math.Pow(pxPos - xPos, 2) + Math.Pow(pyPos - yPos, 2));
                                pieces[x, y].SetZombieHash(Mathf.Min(min, pieces[x, y].ZombieHash));
                            }
                    }
                }
        }

        public bool Simulate()
        {
            for (uint y = 0; y < pieces.GetLength(1); y++)
            {
                for (uint x = 0; x < pieces.GetLength(0); x++)
                    pieces[x, y].Print();

                Console.WriteLine();
            }

            string input = Console.ReadLine();
            SwitchTurn();

            return input.ToLower() != "quit";
        }

        private void SwitchTurn()
        {
            if (players.Count == 0)
            {
                ApplyAI();
                return;
            }

            players[playerIndex].SetTurn(false);

            playerIndex++;
            if (playerIndex >= players.Count)
            {
                playerIndex = 0;
                ApplyAI();
            }

            players[playerIndex].SetTurn(true);
        }

        private void ApplyAI()
        {
            foreach (Player player in NPCs)
            {

            }

            CalculatePlayerHash();

            foreach (Zombie zombie in zombies)
            {
                zombie.Move();

            }
            CalculateZombieHash();

        }

        internal MapPiece GetPiece(ENUM_Direction dir, int x, int y)
        {
            switch (dir)
            {
                case ENUM_Direction.Up:
                    if (y <= 0)
                        return pieces[x, pieces.GetLength(1) - 1];
                    else
                        return pieces[x, y - 1];
                case ENUM_Direction.Down:
                    if (y >= pieces.GetLength(1) - 1)
                        return pieces[x, 0];
                    else
                        return pieces[x, y + 1];
                case ENUM_Direction.Left:
                    if (x <= 0)
                        return pieces[pieces.GetLength(0) - 1, y];
                    else
                        return pieces[x - 1, y];
                case ENUM_Direction.Right:
                    if (x >= pieces.GetLength(0) - 1)
                        return pieces[0, y];
                    else
                        return pieces[x + 1, y];
            }

            return null;
        }
    }
}
