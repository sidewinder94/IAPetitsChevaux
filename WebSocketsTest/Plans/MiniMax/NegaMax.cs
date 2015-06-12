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

        public Tuple<Pawn, int, CaseType> DecisionNegaMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                .Select(st => new Tuple<Tuple<Pawn, int, CaseType>, int>(st, -_DecisionNegaMax(state, depth, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber), st)))
                .ToList();


            return actions.First(a => a.Item2 == actions.Max(m => m.Item2)).Item1;
        }

        private int _DecisionNegaMax(Node state, int depth, int currentPlayerId, Tuple<Pawn, int, CaseType> action)
        {
            if (action == null)
            {
                state.RefreshPawns();
            }
            else
            {
                action.Item1.MoveTo(action.Item3, action.Item2, state.State);
                state.RefreshPawns(action.Item1);
            }

            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);

                state.RollBack();

                return u;
            }

            int[] rolls = new int[6];

            for (var roll = 1; roll < 7; roll++)
            {
                state.Roll = roll;
                rolls[roll - 1] =
                    state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                        .Max(a => -_DecisionNegaMax(state, depth - 1, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber), a));

            }

            state.RollBack();

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

            var nextState = minMax.DecisionNegaMax(currentState, 3, player.Id);

            while (minMax._run)
            {
                currentState.Roll = roll;
                var st = minMax.DecisionNegaMax(currentState, depth, player.Id);

                if (minMax._run)
                {
                    nextState = st;
                }

                depth++;
            }


            if (nextState != null) nextState.Item1.MoveTo(nextState.Item3, nextState.Item2, board);

            return roll;
        }
    }
}