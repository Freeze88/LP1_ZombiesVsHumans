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

        public enum ENUM_Simulation_Result : ushort
        {
            Success = 0,
            GameOver = 1,
            Quit = 2,
        }

        MapPiece[,] pieces;
        List<Player> players = new List<Player>();
        List<Player> NPCs = new List<Player>();
        internal List<Zombie> zombies = new List<Zombie>();
        int playerIndex = 0;

        public Map(uint mapWidth, uint mapHeight, uint playerCount, uint playersToControl, uint zombieCount)
        {
            pieces = new MapPiece[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
                for (int x = 0; x < mapWidth; x++)
                    Add(new EmptySpace(new Vector2(x, y)));

            for (uint i = 0, j = 0; i < playerCount; i++, j++)
                PlacePlayer(j < playersToControl, i == 0);

            for (uint i = 0; i < zombieCount; i++)
                PlaceZombie();

            Update();
        }

        private void PlacePlayer(bool canControl, bool isTurn)
        {
            int posX = Mathf.RandomRange(0, pieces.GetLength(0) - 1);
            int posY = Mathf.RandomRange(0, pieces.GetLength(1) - 1);

            Player playa = new Player(new Vector2(posX, posY), canControl, isTurn);

            if (canControl)
            {
                players.Add(playa);
                playa.OnInfected += OnPlayerInfected;
            }
            else
            {
                NPCs.Add(playa);
                playa.OnInfected += OnNPCInfected;
            }
            Add(playa);

        }

        private void OnPlayerInfected(Player sender)
        {
            players.Remove(sender);
            sender.OnMoved -= OnCharacterMoved;
        }

        private void OnNPCInfected(Player sender)
        {
            NPCs.Remove(sender);
            sender.OnMoved -= OnCharacterMoved;
        }

        private void PlaceZombie()
        {
            int x = 0,
                y = 0;
            do
            {
                x = Mathf.RandomRange(1, pieces.GetLength(0) - 1);
                y = Mathf.RandomRange(1, pieces.GetLength(1) - 1);
            } while (!(pieces[x, y] is EmptySpace));

            Zombie zombie = new Zombie(new Vector2(x, y));
            Add(zombie);
        }

        private void Add(MapPiece piece)
        {
            if (piece is Character auxCharacter)
                auxCharacter.OnMoved -= OnCharacterMoved;

            pieces[piece.Position.X, piece.Position.Y] = piece;

            if (piece is Character character)
                character.OnMoved += OnCharacterMoved;

            if (piece is Zombie zombie && !zombies.Contains(zombie))
                zombies.Add(zombie);
        }

        private void OnCharacterMoved(Character sender, ENUM_Direction direction, Vector2 oldPosition)
        {
            EmptySpace space = new EmptySpace(oldPosition);
            space.SetPlayerHash(pieces[space.Position.X, space.Position.Y].PlayerHash);
            space.SetZombieHash(pieces[space.Position.X, space.Position.Y].ZombieHash);
            Add(space);

            sender.SetZombieHash(pieces[sender.Position.X, sender.Position.Y].ZombieHash);
            sender.SetPlayerHash(pieces[sender.Position.X, sender.Position.Y].PlayerHash);
            Add(sender);
        }

        public ENUM_Simulation_Result Simulate()
        {
            ENUM_Simulation_Result simulationResult = ENUM_Simulation_Result.Success;

            Render();

            string input = "";
            if (players.Count > 0 || NPCs.Count > 0)
                do
                {
                    if (players.Count == 0 || !CanMove)
                        Console.Write("Press Enter To Simulate! or write 'quit' to Exit");
                    else
                        Console.Write("(w, s, a, d) To move the magenta player ; 'quit' to Exit : ");

                    input = Console.ReadLine();
                } while (!ApplyInput(input));

            SwitchTurn();

            if (players.Count == 0 && NPCs.Count == 0)
                simulationResult = ENUM_Simulation_Result.GameOver;

            if (input.ToLower() == "quit")
                simulationResult = ENUM_Simulation_Result.Quit;

            return simulationResult;
        }

        public void Render()
        {
            for (uint y = 0; y < pieces.GetLength(1); y++)
            {
                for (uint x = 0; x < pieces.GetLength(0); x++)
                    pieces[x, y].Print();

                Console.WriteLine();
            }
        }

        private bool ApplyInput(string input)
        {
            if (players.Count == 0 || !CanMove)
                return true;

            if (input.ToLower() == "w")
                return players[playerIndex].Move(this, ENUM_Direction.Up);
            else if (input.ToLower() == "s")
                return players[playerIndex].Move(this, ENUM_Direction.Down);
            else if (input.ToLower() == "a")
                return players[playerIndex].Move(this, ENUM_Direction.Left);
            else if (input.ToLower() == "d")
                return players[playerIndex].Move(this, ENUM_Direction.Right);

            return true;
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
            else
                Update();

            if (players.Count > 0)
                players[playerIndex].SetTurn(true);
        }

        private void ApplyAI()
        {
            foreach (Player player in NPCs.ToArray())
                player.Move(this);
            Player.CalculateHash(this);

            foreach (Zombie zombie in zombies.ToArray())
                zombie.Move(this);
            Zombie.CalculateHash(this);

            Zombie.Infect(this);
        }

        private void Update()
        {
            Player.CalculateHash(this);
            Zombie.CalculateHash(this);
            Zombie.Infect(this);
        }

        internal MapPiece GetPiece(ENUM_Direction dir, Vector2 position)
        {
            switch (dir)
            {
                case ENUM_Direction.Up:
                    if (position.Y == 0)
                        return pieces[position.X, pieces.GetLength(1) - 1];
                    else
                        return pieces[position.X, position.Y - 1];
                case ENUM_Direction.Down:
                    if (position.Y == pieces.GetLength(1) - 1)
                        return pieces[position.X, 0];
                    else
                        return pieces[position.X, position.Y + 1];
                case ENUM_Direction.Left:
                    if (position.X == 0)
                        return pieces[pieces.GetLength(0) - 1, position.Y];
                    else
                        return pieces[position.X - 1, position.Y];
                case ENUM_Direction.Right:
                    if (position.X == pieces.GetLength(0) - 1)
                        return pieces[0, position.Y];
                    else
                        return pieces[position.X + 1, position.Y];
            }

            return null;
        }

        private bool CanMove
        {
            get
            {
                MapPiece topPiece = GetPiece(ENUM_Direction.Up, players[playerIndex].Position);
                MapPiece BottomPiece = GetPiece(ENUM_Direction.Down, players[playerIndex].Position);
                MapPiece leftPiece = GetPiece(ENUM_Direction.Left, players[playerIndex].Position);
                MapPiece rightPiece = GetPiece(ENUM_Direction.Right, players[playerIndex].Position);

                return topPiece is EmptySpace || BottomPiece is EmptySpace || leftPiece is EmptySpace || rightPiece is EmptySpace;
            }
        }
    }
}


