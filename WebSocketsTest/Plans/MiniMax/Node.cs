using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans.MiniMax
{
    class Node
    {
        public List<Player> State;
        public int Roll;


        private List<Player> CloneState()
        {
            return State.Select(p => (Player)p.Clone()).ToList();
        }


        public IEnumerable<Node> GetNextNodes(int playerId = -1)
        {
            if (playerId == -1) playerId = Board.NextPlayer;
            bool moved = false;
            foreach (var p in State.First(player => player.Id == playerId).Pawns)
            {
                if (p.Type.Equals(CaseType.Classic))
                {
                    var newState = CloneState();
                    newState.First(player => player.Id == playerId).Pawns.First(pa => pa.Equals(p)).Move(Roll, newState);
                    moved = true;
                    yield return new Node { State = newState };
                }

                if (p.Type.Equals(CaseType.Classic) &&
                    p.Position ==
                    (Board.Normalize(State.First(player => player.Id == playerId).StartCase - 1))
                    && Roll == 1)
                {
                    var newState = CloneState();
                    var paw =
                        newState.First(player => player.Id == playerId).Pawns.First(pa => pa.Equals(p));
                    paw.MoveTo(CaseType.EndGame, 1, newState);
                    moved = true;
                    yield return new Node { State = newState };

                }

                if (p.Type.Equals(CaseType.EndGame) && (Roll == p.Position + 1))
                {
                    var newState = CloneState();
                    newState
                        .First(player => player.Id == playerId)
                        .Pawns.First(pa => pa.Equals(p))
                        .MoveTo(CaseType.EndGame, Roll, newState);
                    moved = true;
                    yield return new Node { State = newState };
                }

                if (p.Type.Equals(CaseType.Square) && Roll == 6)
                {
                    var newState = CloneState();
                    var cPLayer = newState.First(player => player.Id == playerId);
                    var ennemies = newState.Where(e => e != cPLayer);

                    foreach (var pawn in
                        from e in ennemies
                        where e.Pawns.Any(pa => pa.Position == cPLayer.StartCase && pa.Type == CaseType.Classic)
                        select e.Pawns.First(pa => pa.Position == cPLayer.StartCase && pa.Type == CaseType.Classic))
                    {
                        pawn.MoveTo(CaseType.Square, 0, newState);
                    }
                    var paw =
                        cPLayer.Pawns.First(pa => pa.Equals(p));

                    paw.MoveTo(CaseType.Classic, cPLayer.StartCase, newState);
                    moved = true;
                    yield return new Node { State = newState };
                }
            }
            if (!moved)
            {
                yield return new Node { State = CloneState() };
            }
        }

        public int Evaluate(Player player)
        {
            var enScore = State.Where(pl => pl.Id != player.Id).Sum(p => p.Evaluate);
            return player.Evaluate - enScore;
        }


        public Boolean Any(Func<Player, Boolean> predicate)
        {
            var temp = State.Any(predicate);
            return temp;
        }

    }
}
