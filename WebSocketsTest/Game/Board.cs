using System;
using System.Collections.Generic;
using PetitsChevaux.Plans;
using Random = System.Random;

namespace PetitsChevaux.Game
{
    public class Board
    {
        public static int PawnsPerPlayer = 4;

        private static readonly Random RandomGenerator = new Random();

        public static readonly List<Player> Players = new List<Player>();

        public static int RollDice()
        {
            return RandomGenerator.Next(1, 7);
        }

        public static void GeneratePlayers()
        {
            for (int i = 0; i < 4; i++)
            {
                Players.Add(new Player()
                {
                    NextMove = ((i == 0) ? (Action<Player>)MiniMax.NextMove : Plans.SimpleMinded.NextMove)
                });
            }
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