using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PetitsChevaux.Plans;
using PetitsChevaux.Plans.MiniMax;
using Random = System.Random;

namespace PetitsChevaux.Game
{
    public static class Board
    {
        [UsedImplicitly]
        public static int PawnsPerPlayer = 4;

        [UsedImplicitly]
        public static int PlayerNumber = 4;

        private static readonly Random RandomGenerator = new Random();

        public static readonly List<Player> Players = new List<Player>();

        private static int _lastPlayer = 0;

        public static int NextPlayer
        {
            get { return Normalize((_lastPlayer), 4); }
        }

        public static int RollDice()
        {
            return RandomGenerator.Next(1, 7);
        }

        public static void GeneratePlayers()
        {
            for (var i = 0; i < PlayerNumber; i++)
            {
                Players.Add(new Player()
                {
                    NextMove = ((i == 0) ? (Action<Player>)MiniMax.NextMove : Plans.SimpleMinded.NextMove)
                });
            }
        }

        public static void NextTurn()
        {
            Players[Normalize(_lastPlayer++, 4)].Play();
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

                if (i < (against + @base)) return i;

                if (i >= (against + @base))
                {
                    var t = i - against;
                    if (t < (against + @base)) return t;
                    i = t;
                    continue;
                }

                break;
            }

            throw new ArgumentOutOfRangeException("i", i, "should be > " + @base);
        }
    }
}