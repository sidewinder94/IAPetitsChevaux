using System;
using System.Collections.Generic;
using System.Linq;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    public static class MiniMax
    {

        private static int Algorithm(Node node, int depth, Boolean maximizingPlayer, int playerId = 0)
        {
            int bestValue;
            if (depth == 0 || node.Any(p => p.Won))
            {
                return Evaluate(node);
            }

            if (maximizingPlayer)
            {
                bestValue = int.MinValue;
                int[] rolls = new int[6];

                //Handling chance nodes
                for (var roll = 1; roll < 7; roll++)
                {
                    var rollValue = 0;
                    foreach (var child in node.GetNextNodes(roll, Board.Normalize(playerId, Board.PlayerNumber)))
                    {
                        rollValue += Algorithm(child, depth - 1, false, ++playerId);
                    }
                    rolls[roll - 1] = rollValue;
                }

                var val = (int)rolls.Average(r => r);

                bestValue = Math.Max(bestValue, val);

                return bestValue;
            }
            else
            {
                bestValue = int.MaxValue;

                int[] rolls = new int[6];

                //Handling chance nodes
                for (var roll = 1; roll < 7; roll++)
                {
                    var rollValue = 0;
                    foreach (var child in node.GetNextNodes(roll, Board.Normalize(playerId, Board.PlayerNumber)))
                    {
                        rollValue += Algorithm(child, depth - 1, false, ++playerId);
                    }
                    rolls[roll - 1] = rollValue;
                }

                int val = (int)rolls.Average(r => r);

                bestValue = Math.Min(bestValue, val);
                return bestValue;
            }

        }


        public static int NextMove(Player player)
        {
            Node currentState = new Node { State = Board.Players };
            int bestResult = Algorithm(currentState, 4, true);
            int roll = Board.RollDice();
            var nextNodes = currentState.GetNextNodes(roll, player.Id);
            if (nextNodes.Count() != 0)
            {
                var nextNode = nextNodes.FirstOrDefault(n => Evaluate(n) == bestResult);
                player.Pawns.Clear();
                player.Pawns.AddRange(nextNode.State.First(p => p.Id == player.Id).Pawns);
            }
            return roll;
        }

        private static int Evaluate(Node state)
        {
            return state.State[0].Evaluate();
        }

    }
}