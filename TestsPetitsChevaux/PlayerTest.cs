using System;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetitsChevaux.Game;
using PetitsChevaux.Plans.MiniMax;

namespace TestsPetitsChevaux
{
    [TestClass]
    public class PlayerTest
    {

        private Player _startPlayer = null;

        [TestInitialize]
        public void TestInit()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board.GeneratePlayers();
            _startPlayer = Board.Players.First(pl => pl.Id == 0);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _startPlayer = null;
        }

        [TestMethod]
        public void TestEvaluationClean()
        {
            Assert.AreEqual(0, _startPlayer.Evaluate);
        }

        [TestMethod]
        public void TestEvaluationOneOut()
        {
            _startPlayer.Pawns[0].MoveTo(CaseType.Classic, 0);
            var eval = _startPlayer.Evaluate;
            Assert.AreEqual(175616, eval, eval);
        }

        [TestMethod]
        public void TestEvaluationOneOut8()
        {
            _startPlayer.Pawns[0].MoveTo(CaseType.Classic, 8);
            var eval = _startPlayer.Evaluate;
            Assert.AreEqual(64, eval, eval);
        }

        [TestMethod]
        public void TestEvaluationTwoOut8And20()
        {
            _startPlayer.Pawns[0].MoveTo(CaseType.Classic, 8);
            _startPlayer.Pawns[1].MoveTo(CaseType.Classic, 20);

            var eval = _startPlayer.Evaluate;
            Assert.AreEqual(464, eval, eval);
        }

        [TestMethod]
        public void TestEvaluationTwoOut2And6End()
        {
            _startPlayer.Pawns[0].MoveTo(CaseType.EndGame, 2);
            _startPlayer.Pawns[1].MoveTo(CaseType.EndGame, 6);

            var eval = _startPlayer.Evaluate;
            Assert.AreEqual(int.MaxValue, eval, eval);
        }

    }
}
