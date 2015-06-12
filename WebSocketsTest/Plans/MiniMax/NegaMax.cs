using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using PetitsChevaux.Game;
using System.Timers;

namespace PetitsChevaux.Plans.MiniMax
{
    public class NegaMax
    {
        private bool _run = true;
        private int _playerId = 0;

        public Tuple<Pawn, int, CaseType> DecisionNegaMax(Node state, int depth, int currentPlayerId)
        {
            var actions = state.GetNextNodes(Board.Normalize(currentPlayerId, Board.PlayerNumber))
                .Select(st => new Tuple<Tuple<Pawn, int, CaseType>, int>(st, -_DecisionNegaMax(state, depth, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber), st, int.MinValue, int.MaxValue)))
                .ToList();


            return actions.First(a => a.Item2 == actions.Max(m => m.Item2)).Item1;
        }

        private int _DecisionNegaMax(Node state, int depth, int currentPlayerId, Tuple<Pawn, int, CaseType> action, int alpha, int beta)
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


            int[] temp = new int[6];
            foreach (var e in Enumerable.Range(0, 6))
            {
                temp[e] = _playerId == currentPlayerId ? int.MaxValue : int.MinValue;
            }


            int best = int.MinValue;

            foreach (var pa in state.State.First(p => p.Id == currentPlayerId).Pawns)
            {
                for (int i = 0; i < 5; i++)
                {
                    state.Roll = i + 1;

                    var actions = state.GetActions(pa, currentPlayerId).ToList();
                    var utils = actions.Select(a => EvaluateAction(a, state));

                    var util =
                        actions.Zip(utils, (a, u) => new Tuple<Tuple<Pawn, int, CaseType>, int>(a, u))
                            .OrderByDescending(t => t.Item2)
                            .First();


                    var result = -_DecisionNegaMax(state, depth - 1, Board.Normalize(currentPlayerId + 1, Board.PlayerNumber),
                                util.Item1, alpha, beta);

                    temp[i] = result;

                    if (currentPlayerId == _playerId)
                    {
                        if (temp.Average() >= beta)
                        {
                            state.RollBack();
                            return (int)Math.Round(temp.Average());
                        }
                    }
                    else
                    {
                        if (temp.Average() <= alpha)
                        {
                            state.RollBack();
                            return (int)Math.Round(temp.Average());
                        }
                    }
                }

                best = Math.Max((int)Math.Round(temp.Average()), best);

                if (currentPlayerId == _playerId)
                {
                    alpha = Math.Max((int)Math.Round(temp.Average()), alpha);

                }
                else
                {
                    beta = Math.Min((int)Math.Round(temp.Average()), beta);
                }

            }

            state.RollBack();

            return best;
        }



        private int EvaluateAction(Tuple<Pawn, int, CaseType> action, Node state)
        {
            if (action != null)
            {
                action.Item1.MoveTo(action.Item3, action.Item2, state.State);
                state.RefreshPawns(action.Item1);
            }
            else
            {
                state.RefreshPawns();
            }

            var result = Utility(state, _playerId);

            state.RollBack();

            return result;
        }


        private int Utility(Node state, int currentPlayerId)
        {
            return state.Evaluate(state.State[Board.Normalize(currentPlayerId, Board.PlayerNumber)]);
        }


        public static int NextMove(Player player, List<Player> board)
        {
            var minMax = new NegaMax { _playerId = player.Id };


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