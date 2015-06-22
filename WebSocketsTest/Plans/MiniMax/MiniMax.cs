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

        public Contracts.Action DecisionMiniMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, state.State.Count))
                .Select(st => new Tuple<Contracts.Action, int>(st, ValeurMin(state, depth, Board.Normalize(currentPlayerId + 1, state.State.Count), st)))
                .ToList();

            return actions.First(a => a.Item2 == actions.Max(m => m.Item2)).Item1;
        }

        private int ValeurMax(Node state, int depth, int currentPlayerId, Contracts.Action action)
        {
            if (action == null)
            {
                state.RefreshPawns();
            }
            else
            {
                action.Subject.MoveTo(action.Type, action.Position, state.State);
                state.RefreshPawns(action.Subject);
            }

            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);

                state.RollBack();

                return currentPlayerId != 0 ? -u : u;
            }
            int[] rolls = new int[6];
            for (var roll = 1; roll < 7; roll++)
            {
                state.Roll = roll;
                rolls[roll - 1] =
                    state.GetNextNodes(Board.Normalize(currentPlayerId, state.State.Count))
                        .Max(a => ValeurMin(state, depth - 1, Board.Normalize(currentPlayerId + 1, state.State.Count), a));

            }

            state.RollBack();

            return (int)Math.Round(rolls.Average());
        }

        private int ValeurMin(Node state, int depth, int currentPlayerId, Contracts.Action action)
        {
            if (action == null)
            {
                state.RefreshPawns();
            }
            else
            {
                action.Subject.MoveTo(action.Type, action.Position, state.State);
                state.RefreshPawns(action.Subject);
            }

            if (!_run || (depth == 0 || state.Any(p => p.Won)))
            {
                int u = Utility(state, currentPlayerId);

                state.RollBack();

                return currentPlayerId != 0 ? -u : u;
            }

            int[] rolls = new int[6];
            for (var roll = 1; roll < 7; roll++)
            {
                state.Roll = roll;
                rolls[roll - 1] =
                    state.GetNextNodes(Board.Normalize(currentPlayerId, state.State.Count))
                        .Min(a => ValeurMax(state, depth - 1, Board.Normalize(currentPlayerId + 1, state.State.Count), a));

            }

            state.RollBack();

            return (int)Math.Round(rolls.Average());
        }


        private static int Utility(Node state, int evaluatedPlayer, [CallerMemberName] string t = "", [CallerLineNumber] int l = 0)
        {
            return state.Evaluate(state.State[Board.Normalize(evaluatedPlayer, state.State.Count)]);
        }

        public static Contracts.Action NextMove(Player player, List<Player> board, int roll)
        {
            var minMax = new MiniMax();

            Node currentState = new Node { State = board, Roll = roll };

            int depth = 4;


            var timer = new Timer(500)
            {
                AutoReset = false,
            };

            timer.Elapsed += (sender, args) => minMax._run = false;

            timer.Start();

            var nextState = minMax.DecisionMiniMax(currentState, 3, player.Id);

            while (minMax._run)
            {
                var st = minMax.DecisionMiniMax(currentState, depth, player.Id);

                if (minMax._run)
                {
                    nextState = st;
                }

                depth++;
            }

            nextState.Subject.MoveTo(nextState.Type, nextState.Position, board);

            return nextState;
        }

    }
}