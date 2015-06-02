using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetitsChevaux.Game;
using PetitsChevaux.Plans.MiniMax;

namespace TestsPetitsChevaux
{
    [TestClass]
    public class NodeTest
    {

        private Node _startNode = null;

        [TestInitialize]
        public void TestInit()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board board = new Board();

            _startNode = new Node { State = board.Players };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _startNode = null;
        }

        [TestMethod]
        public void TestGetNextNodesTryToGetOut()
        {
            _startNode.Roll = 1;
            var output = _startNode.GetNextNodes(0);

            Assert.IsFalse(output.Any(n => n != null), "A Pawn got out with a roll of 1");

            _startNode.Roll = 6;
            output = _startNode.GetNextNodes(0);


            foreach (var action in output)
            {
                if (action != null)
                {
                    action.Item1.MoveTo(action.Item3, action.Item2, _startNode.State);
                    _startNode.RefreshPawns(action.Item1);
                }
                else
                {
                    _startNode.RefreshPawns();
                }

                _startNode.State.ForEach(pl =>
                {
                    var count = pl.Pawns.Count(pa => pa.Position == pl.StartCase && pa.Type == CaseType.Classic);

                    if (pl.Id != 0 && count != 0) Assert.Fail("Player 2 Pawn got out while it was Player 1 turn");

                    if (pl.Id == 0 && count == 0) Assert.Fail("Player 1 Pawn didn't got out with a roll of 6");

                    if (pl.Id == 0 && count > 1) Assert.Fail("Player 1 got " + count + " pawns out in 1 turn");
                });

                _startNode.RollBack();
            }

        }


        [TestMethod]
        public void TestGetNextNodesPriorityOutBeforeMove()
        {
            var player = _startNode.State.First(pl => pl.Id == 0);

            player.Pawns[0].MoveTo(CaseType.Classic, 13, _startNode.State);
            player.Pawns[1].MoveTo(CaseType.Classic, 27, _startNode.State);
            player.Pawns[2].MoveTo(CaseType.Classic, 41, _startNode.State);

            _startNode.State.First(pl => pl.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 51, _startNode.State);

            _startNode.Roll = 6;

            var output = _startNode.GetNextNodes(0);

            var eval = output.Select(n =>
            {
                if (n != null)
                {
                    n.Item1.MoveTo(n.Item3, n.Item2, _startNode.State);
                    _startNode.RefreshPawns(n.Item1);
                }
                else
                {
                    _startNode.RefreshPawns();
                }

                var r = new Tuple<Tuple<Pawn, int, CaseType>, int>(n, _startNode.Evaluate(_startNode.State.Find(p => p.Id == 0)));

                _startNode.RollBack();

                return r;
            }).ToList();

            var best = eval.First(a => a.Item2 == eval.Max(m => m.Item2)).Item1;
            best.Item1.MoveTo(best.Item3, best.Item2, _startNode.State);

            var testedPawn = _startNode.State.First(pl => pl.Id == 0).Pawns[3];

            Assert.IsTrue(testedPawn.Position == 0, "Pawn not in initial position");
            Assert.IsTrue(testedPawn.Type == CaseType.Classic, "Pawn did not get out");


        }
    }
}
