using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PetitsChevaux.Plans;
using PetitsChevaux.Plans.MiniMax;
using Random = System.Random;

namespace PetitsChevaux.Game
{
    public static class Board
    {
        [UsedImplicitly]
        public static int PawnsPerPlayer = 4;

        [UsedImplicitly]
        public static int PlayerNumber = 4;

        private static readonly Random RandomGenerator = new Random();

        public static readonly List<Player> Players = new List<Player>();

        private static int _lastPlayer = 0;

        public static int NextPlayer
        {
            get { return Normalize((_lastPlayer), 4); }
        }

        public static int RollDice()
        {
            return RandomGenerator.Next(1, 7);
        }

        public static void GeneratePlayers()
        {
            Player.Reset();
            Players.Clear();
            for (var i = 0; i < PlayerNumber; i++)
            {
                Players.Add(new Player()
                {
                    NextMove = ((i == 0) ? (Func<Player, int>)MiniMax.NextMove : SimpleMinded.NextMove)
                });
            }
        }

        public static Dictionary<String, int> NextTurn()
        {
            int player = Normalize(_lastPlayer++, Board.PlayerNumber);
            int roll = Players[player].Play();

            var result = new Dictionary<String, int>();
            Players.ForEach(p =>
            {
                var squareCounter = 0;
                p.Pawns.ForEach(pawn =>
                {
                    if (pawn.Type == CaseType.Classic)
                    {
                        if (!result.ContainsKey(String.Format(CaseType.Classic.ToString(), pawn.Position)))
                        {
                            result.Add(String.Format(CaseType.Classic.ToString(), pawn.Position), p.Id + 1);
                        }
                    }
                    else if (pawn.Type == CaseType.EndGame)
                    {
                        if (!result.ContainsKey(String.Format(CaseType.EndGame.ToString(), p.Id + 1, pawn.Position)))
                        {
                            result.Add(String.Format(CaseType.EndGame.ToString(), p.Id + 1, pawn.Position), 0);
                        }
                    }
                    else if (pawn.Type == CaseType.Square)
                    {
                        squareCounter++;
                    }
                });
                result.Add(String.Format(CaseType.Square.ToString(), p.Id + 1), squareCounter);
            });
            result.Add("Player rolling : ", player);
            result.Add("rolled", roll);
            return result;
        }

        public static int Normalize(int i, int against = 56, int @base = 0)
        {
            while (true)
            {
                if (i < 0)
                {
                    var t = (against + @base) + i;
                    if (t > 0) return t;
                    i = t;
                    continue;
                }

                if (i >= (against + @base))
                {
                    var t = i - against;
                    if (t < (against + @base)) return t;
                    i = t;
                    continue;
                }

                if (i < (against + @base)) return i;

                break;
            }

            throw new ArgumentOutOfRangeException("i", i, "should be > " + @base);
        }
    }
}