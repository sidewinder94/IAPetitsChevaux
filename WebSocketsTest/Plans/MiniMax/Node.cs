using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    public class Node
    {
        public List<Player> State;
        public int Roll;


        private List<Player> CloneState()
        {
            return State.Select(p => (Player)p.Clone()).ToList();
        }


        public Node RollBack(int moves = 1)
        {
            State.ForEach(pl => pl.Pawns.ForEach(pa =>
            {
                pa.RollBack(moves);
            }));

            return this;
        }

        public Node RefreshPawns(Pawn moved = null)
        {
            State.ForEach(p => p.Pawns.ForEach(pa =>
            {
                if (moved == null)
                {
                    pa.NoMove();
                    return;
                }

                if (pa != moved) pa.NoMove();
            }));

            return this;
        }

        public IEnumerable<Tuple<Pawn, int, CaseType>> GetNextNodes(int playerId = -1)
        {
            int roll = Roll;

            if (playerId == -1) throw new ArgumentException("Player ID Invalid", "playerId");
            bool moved = false;
            foreach (var p in State.First(player => player.Id == playerId).Pawns)
            {
                if (p.Type.Equals(CaseType.Classic))
                {
                    moved = true;
                    yield return new Tuple<Pawn, int, CaseType>(p, Board.Normalize(p.Position + roll), CaseType.Classic);
                }

                if (p.Type.Equals(CaseType.Classic) &&
                    p.Position ==
                    (Board.Normalize(State.First(player => player.Id == playerId).StartCase - 1))
                    && roll == 1)
                {
                    moved = true;
                    yield return new Tuple<Pawn, int, CaseType>(p, 1, CaseType.EndGame);

                }

                if (p.Type.Equals(CaseType.EndGame) && (roll == p.Position + 1))
                {
                    moved = true;
                    yield return new Tuple<Pawn, int, CaseType>(p, roll, CaseType.EndGame);
                }

                if (p.Type.Equals(CaseType.Square) && roll == 6)
                {
                    var cPLayer = State.First(player => player.Id == playerId);
                    moved = true;
                    yield return new Tuple<Pawn, int, CaseType>(p, cPLayer.StartCase, CaseType.Classic);
                }
            }
            if (!moved)
            {
                yield return null;
            }
        }

        public int Evaluate(Player player)
        {
            var enScore = State.First(p => p.Id == Board.Normalize(player.Id + 1, Board.PlayerNumber)).Evaluate;
            return player.Evaluate - enScore;
        }


        public Boolean Any(Func<Player, Boolean> predicate)
        {
            var temp = State.Any(predicate);
            return temp;
        }


    }
}
