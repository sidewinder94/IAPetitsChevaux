﻿using System;
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


        private Node RefreshPawns(Pawn moved = null)
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

        public IEnumerable<Node> GetNextNodes(int playerId = -1)
        {
            if (playerId == -1) throw new ArgumentException("PLayer ID Invalid", "playerId");
            bool moved = false;
            foreach (var p in State.First(player => player.Id == playerId).Pawns)
            {
                if (p.Type.Equals(CaseType.Classic))
                {
                    var newState = CloneState();
                    var movedPawn = newState.First(player => player.Id == playerId).Pawns.First(pa => pa.Equals(p)).Move(Roll, newState);
                    moved = true;
                    yield return new Node { State = newState }.RefreshPawns(movedPawn);
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
                    yield return new Node { State = newState }.RefreshPawns(paw);

                }

                if (p.Type.Equals(CaseType.EndGame) && (Roll == p.Position + 1))
                {
                    var newState = CloneState();
                    var paw = newState
                        .First(player => player.Id == playerId)
                        .Pawns.First(pa => pa.Equals(p))
                        .MoveTo(CaseType.EndGame, Roll, newState);
                    moved = true;
                    yield return new Node { State = newState }.RefreshPawns(paw);
                }

                if (p.Type.Equals(CaseType.Square) && Roll == 6)
                {
                    var newState = CloneState();
                    var cPLayer = newState.First(player => player.Id == playerId);

                    var paw =
                        cPLayer.Pawns.First(pa => pa.Equals(p));

                    paw.MoveTo(CaseType.Classic, cPLayer.StartCase, newState);
                    moved = true;
                    yield return new Node { State = newState }.RefreshPawns(paw);
                }
            }
            if (!moved)
            {
                yield return new Node { State = CloneState() }.RefreshPawns();
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
