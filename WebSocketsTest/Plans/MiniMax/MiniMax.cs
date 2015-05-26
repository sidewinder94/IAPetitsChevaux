using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    public class MiniMax
    {

        private bool _run = true;

        public Node DecisionMiniMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                .Select(st => new Tuple<Node, int>(st, ValeurMin(st, depth, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber))))
                .ToList();

            return actions.First(a => a.Item2 == actions.Max(m => m.Item2)).Item1;
        }

        private int ValeurMax(Node state, int depth, int currentPlayerId)
        {
            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);

                return currentPlayerId != 0 ? -u : u;
            }
            int[] rolls = new int[6];
            for (var roll = 1; roll < 7; roll++)
            {
                state.Roll = roll;
                rolls[roll - 1] =
                    state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                        .Max(a => ValeurMin(a, depth - 1, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber)));

            }

            return (int)Math.Round(rolls.Average());
        }

        private int ValeurMin(Node state, int depth, int currentPlayerId)
        {
            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);
                Debug.Print(u.ToString());
                return currentPlayerId != 0 ? -u : u;
            }

            int[] rolls = new int[6];
            for (var roll = 1; roll < 7; roll++)
            {
                state.Roll = roll;
                rolls[roll - 1] =
                    state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                        .Min(a => ValeurMax(a, depth - 1, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber)));

            }

            return (int)Math.Round(rolls.Average());
        }


        private static int Utility(Node state, int evaluatedPlayer, [CallerMemberName] string t = "", [CallerLineNumber] int l = 0)
        {
            return state.Evaluate(state.State[Board.Normalize(evaluatedPlayer, Board.PlayerNumber)]);
        }

        public static int NextMove(Player player, List<Player> board)
        {
            MiniMax minMax = new MiniMax();

            int roll = Board.RollDice();

            Node currentState = new Node { State = board, Roll = roll };

            int depth = 4;


            var timer = new Timer(500)
            {
                AutoReset = false,
            };

            timer.Elapsed += (sender, args) => minMax._run = false;

            timer.Start();

            Node nextState = minMax.DecisionMiniMax(currentState, 3, player.Id);

            while (minMax._run)
            {
                var st = minMax.DecisionMiniMax(currentState, depth, player.Id);

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