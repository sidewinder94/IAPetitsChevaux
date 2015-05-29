using System;
using System.Collections.Generic;
using System.Linq;
using PetitsChevaux.Game;
using System.Timers;

namespace PetitsChevaux.Plans.MiniMax
{
    public class NegaMax
    {
        private bool _run = true;

        public Node DecisionNegaMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                .Select(st => new Tuple<Node, int>(st, -_DecisionNegaMax(st, depth, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber))))
                .ToList();

            return actions.First(a => a.Item2 == actions.Max(m => m.Item2)).Item1;
        }

        private int _DecisionNegaMax(Node state, int depth, int currentPlayerId)
        {
            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);

                return u;
            }

            int[] rolls = new int[6];

            for (var roll = 1; roll < 7; roll++)
            {
                state.Roll = roll;
                rolls[roll - 1] =
                    state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                        .Max(a => -_DecisionNegaMax(a, depth - 1, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber)));

            }

            return (int)Math.Round(rolls.Average());
        }

        private int Utility(Node state, int currentPlayerId)
        {
            return state.Evaluate(state.State[Board.Normalize(currentPlayerId, Board.PlayerNumber)]);
        }


        public static int NextMove(Player player, List<Player> board)
        {
            var minMax = new NegaMax();

            int roll = Board.RollDice();

            Node currentState = new Node { State = board, Roll = roll };

            int depth = 4;


            var timer = new Timer(500)
            {
                AutoReset = false,
            };

            timer.Elapsed += (sender, args) => minMax._run = false;

            timer.Start();

            Node nextState = minMax.DecisionNegaMax(currentState, 3, player.Id);

            while (minMax._run)
            {
                var st = minMax.DecisionNegaMax(currentState, depth, player.Id);

                if (minMax._run)
                {
                    nextState = st;
                }

                depth++;
            }

            board.ForEach(p => p.Pawns.Clear());
            board.ForEach(p => p.Pawns.AddRange(nextState.State.First(pl => pl.Id == p.Id).Pawns));
            return roll;
        }
    }
}