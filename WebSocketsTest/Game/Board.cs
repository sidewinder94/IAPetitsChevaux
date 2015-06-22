using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PetitsChevaux.Plans;
using PetitsChevaux.Plans.MiniMax;
using Random = System.Random;

namespace PetitsChevaux.Game
{
    public class Board
    {
        [UsedImplicitly]
        public static int PawnsPerPlayer = 4;

        [UsedImplicitly]
        public static int PlayerNumber = 4;
        public readonly List<Player> Players = new List<Player>();


        public Board()
        {
            GeneratePlayers();
        }
        public Board(List<Player> players)
        {
            this.Players = players;
        }

        public void GeneratePlayers()
        {
            Players.Clear();
            for (var i = 0; i < PlayerNumber; i++)
            {
                Players.Add(new Player(i)
                {
                    NextMove = ((i == 0) ? (Func<Player, List<Player>, int, Contracts.Action>)NegaMax.NextMove : SimpleMinded.NextMove)
                });
            }
        }
        public Contracts.Action NextTurn(int playerId, int roll)
        {

            int player = Normalize(playerId, Players.Count);
            var result = Players[player].Play(Players, roll);

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