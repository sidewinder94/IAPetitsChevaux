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



        private List<Player> CloneState()
        {
            return State.Select(p => (Player)p.Clone()).ToList();
        }




        public IEnumerable<Node> GetNextNodes(int roll, int playerId = -1)
        {
            if (playerId == -1) playerId = Board.NextPlayer;

            foreach (var p in State.First(player => player.Id == playerId).Pawns)
            {
                if (p.Type.Equals(CaseType.Classic))
                {
                    var newState = CloneState();
                    newState.First(player => player.Id == playerId).Pawns.First(pa => pa.Equals(p)).Move(roll, newState);
                    yield return new Node { State = newState };
                }

                if (p.Type.Equals(CaseType.Classic) &&
                    p.Position ==
                    (Board.Normalize(State.First(player => player.Id == playerId).StartCase - 1))
                    && roll == 1)
                {
                    var newState = CloneState();
                    var paw =
                        newState.First(player => player.Id == playerId).Pawns.First(pa => pa.Equals(p));
                    paw.Position = 1;
                    paw.Type = CaseType.EndGame;

                    yield return new Node { State = newState };

                }

                if (p.Type.Equals(CaseType.EndGame) && (roll == p.Position + 1))
                {
                    var newState = CloneState();
                    newState
                        .First(player => player.Id == playerId)
                        .Pawns.First(pa => pa.Equals(p))
                        .Position++;
                    yield return new Node { State = newState };
                }

                if (p.Type.Equals(CaseType.Square) && roll == 6)
                {
                    var newState = CloneState();
                    var cPLayer = newState.First(player => player.Id == playerId);
                    var paw =
                        cPLayer.Pawns.First(pa => pa.Equals(p));
                    paw.Position = cPLayer.StartCase;
                    paw.Type = CaseType.Classic;

                    yield return new Node { State = newState };
                }
            }
        }


        public int Value(Player player)
        {
            return State.First(p => p.Id == player.Id).Evaluate();
        }

        public Boolean Any(Func<Player, Boolean> predicate)
        {
            return State.Any(predicate);
        }

    }
}
