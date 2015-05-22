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
            Board.GeneratePlayers();
            _startNode = new Node { State = Board.Players };
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

            Assert.IsTrue(output.All(n => n.State.All(pl => pl.Pawns.All(p => p.Position == 0 && p.Type == CaseType.Square))),
                "A Pawn got out with a roll of 1");

            _startNode.Roll = 6;
            output = _startNode.GetNextNodes(0);


            foreach (var node in output)
            {
                node.State.ForEach(pl =>
                {
                    var count = pl.Pawns.Count(pa => pa.Position == pl.StartCase && pa.Type == CaseType.Classic);

                    if (pl.Id != 0 && count != 0) Assert.Fail("Player 2 Pawn got out while it was Player 1 turn");

                    if (pl.Id == 0 && count == 0) Assert.Fail("Player 1 Pawn didn't got out with a roll of 6");

                    if (pl.Id == 0 && count > 1) Assert.Fail("Player 1 got " + count + " pawns out in 1 turn");
                });
            }

        }


        [TestMethod]
        public void TestGetNextNodesPriorityOutBeforeMove()
        {
            var player = _startNode.State.First(pl => pl.Id == 0);

            player.Pawns[0].MoveTo(CaseType.Classic, 13);
            player.Pawns[1].MoveTo(CaseType.Classic, 27);
            player.Pawns[2].MoveTo(CaseType.Classic, 41);

            _startNode.State.First(pl => pl.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 51);

            _startNode.Roll = 6;

            var output = _startNode.GetNextNodes(0);

            var eval = output.Select(n => new Tuple<Node, int>(n, n.Evaluate(n.State.Find(p => p.Id == 0)))).ToList();

            Node best = eval.First(a => a.Item2 == eval.Max(m => m.Item2)).Item1;

            var testedPawn = best.State.First(pl => pl.Id == 0).Pawns[3];

            Assert.IsTrue(testedPawn.Position == 0, "Pawn not in initial position");
            Assert.IsTrue(testedPawn.Type == CaseType.Classic, "Pawn did not get out");


        }
    }
}
