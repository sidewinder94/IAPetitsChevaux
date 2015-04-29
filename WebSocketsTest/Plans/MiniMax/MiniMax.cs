using System;
using System.Collections.Generic;
using System.Linq;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    public static class MiniMax
    {

        private static int Algorithm(Node node, int depth, Boolean maximizingPlayer)
        {
            int bestValue;
            if (depth == 0 || node.Any(p => p.Won))
            {
                return Evaluate(node);
            }

            if (maximizingPlayer)
            {
                int val = 0;
                bestValue = int.MinValue;
                foreach (var child in node)
                {
                    val = Algorithm(child, depth - 1, false);
                    bestValue = (bestValue > val) ? bestValue : val;
                }

                return bestValue;
            }
            else
            {
                bestValue = int.MaxValue;
                int val = 0;
                foreach (var child in node)
                {
                    val = Algorithm(child, depth - 1, true);
                    bestValue = (bestValue > val) ? val : bestValue;
                }
                return bestValue;
            }

        }


        public static void NextMove(Player player)
        {
            throw new System.NotImplementedException();
        }

        private static int Evaluate(List<Player> state)
        {
            throw new NotImplementedException();
        }

    }
}