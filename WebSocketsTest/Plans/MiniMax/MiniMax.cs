using System;
using System.Linq;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    public static class MiniMax
    {
        private static Node DecisionMiniMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                .Select(st => new Tuple<Node, int>(st, ValeurMin(st, depth - 1, Board.Normalize(currentPlayerId, Board.PlayerNumber))))
                .ToList();

            return actions.First(a => a.Item2 == actions.Min(m => m.Item2)).Item1;
        }

        private static int ValeurMax(Node state, int depth, int currentPlayerId)
        {
            if (depth == 0 || state.Any(p => p.Won))
            {
                return Utility(state, currentPlayerId);
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
            if (depth == 0 || state.Any(p => p.Won))
            {
                return Utility(state, currentPlayerId);
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


        private static int Utility(Node state, int evaluatedPlayer)
        {
            return state.Evaluate(state.State[Board.Normalize(evaluatedPlayer, Board.PlayerNumber)]);
        }

        public static int NextMove(Player player)
        {
            int roll = Board.RollDice();
            Node currentState = new Node { State = Board.Players, Roll = roll };
            var nextState = DecisionMiniMax(currentState, 3, player.Id);
            Board.Players.ForEach(p => p.Pawns.Clear());
            Board.Players.ForEach(p => p.Pawns.AddRange(nextState.State.First(pl => pl.Id == p.Id).Pawns));
            return roll;
        }

    }
}