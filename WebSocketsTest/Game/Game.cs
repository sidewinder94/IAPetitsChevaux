using System;

namespace PetitsChevaux.Game
{
    public class Game
    {
        public static int PawnsPerPlayer = 4;

        private static readonly Random RandomGenerator = new Random();

        public static int RollDice()
        {
            return RandomGenerator.Next(1, 7);
        }


        public static int Normalize(int i, int against = 56, int @base = 0)
        {
            while (true)
            {
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