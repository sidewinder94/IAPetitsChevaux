using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PetitsChevaux.Plans;
using PetitsChevaux.Plans.MiniMax;
using PetitsChevaux.Contract;
using Random = System.Random;

namespace PetitsChevaux.Game
{
    public class Board
    {
        [UsedImplicitly]
        public static int PawnsPerPlayer = 4;

        [UsedImplicitly]
        public static int PlayerNumber = 4;

        private static readonly Random RandomGenerator = new Random();

        public readonly List<Player> Players = new List<Player>();

        private int _lastPlayer;

        public int PlayerId = -1;

        public Board()
        {
            GeneratePlayers();
        }

        public static int RollDice()
        {
            return RandomGenerator.Next(1, 7);
        }

        public void GeneratePlayers()
        {
            Players.Clear();
            for (var i = 0; i < PlayerNumber; i++)
            {
                Players.Add(new Player(i)
                {
                    NextMove = ((i == 0) ? (Func<Player, List<Player>, int, int>)MiniMax.NextMove : SimpleMinded.NextMove)
                });
            }
        }

        public ContractBase NextTurn(int r = -1)
        {
            int player = (PlayerId == -1) ? Normalize(_lastPlayer++, Board.PlayerNumber) : PlayerId;
            int roll = Players[player].Play(Players, r);

            var result = new Send() { Players = this.Players };

            return result;
        }

        public static int Normalize(int i, int against = 56, int @base = 0)
        {
            while (true)
            {
                if (i < 0)
                {
                    var t = (against + @base) + i;
                    if (t > 0) return t;
                    i = t;
                    continue;
                }

                if (i >= (against + @base))
                {
                    var t = i - against;
                    if (t < (against + @base)) return t;
                    i = t;
                    continue;
                }

                if (i < (against + @base)) return i;

                break;
            }

            throw new ArgumentOutOfRangeException("i", i, "should be > " + @base);
        }
    }
}