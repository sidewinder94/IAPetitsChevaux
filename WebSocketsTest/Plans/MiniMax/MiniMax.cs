using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    public static class MiniMax
    {



        private static bool _run = true;

        public static Node DecisionMiniMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                .Select(st => new Tuple<Node, int>(st, ValeurMin(st, depth, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber))))
                .ToList();

            return actions.First(a => a.Item2 == actions.Max(m => m.Item2)).Item1;
        }

        private static int ValeurMax(Node state, int depth, int currentPlayerId)
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

        private static int ValeurMin(Node state, int depth, int currentPlayerId)
        {
            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);
                Debug.Print(u.ToString());
                return u;
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

        public static int NextMove(Player player)
        {
            _run = true;

            int roll = Board.RollDice();

            Node currentState = new Node { State = Board.Players, Roll = roll };

            int depth = 4;


            var timer = new Timer(500)
            {
                AutoReset = false,
            };

            timer.Elapsed += (sender, args) => _run = false;

            timer.Start();

            Node nextState = DecisionMiniMax(currentState, 3, player.Id);

            while (_run)
            {
                var st = DecisionMiniMax(currentState, depth, player.Id);

                if (_run)
                {
                    nextState = st;
                }

                depth++;
            }


            Board.Players.ForEach(p => p.Pawns.Clear());
            Board.Players.ForEach(p => p.Pawns.AddRange(nextState.State.First(pl => pl.Id == p.Id).Pawns));
            return roll;
        }

    }
}