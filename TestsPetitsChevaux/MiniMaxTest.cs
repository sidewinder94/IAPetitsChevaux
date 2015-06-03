using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetitsChevaux.Game;
using PetitsChevaux.Plans.MiniMax;

namespace TestsPetitsChevaux
{
    [TestClass]
    public class MiniMaxTest
    {
        private List<Player> _board = null;
        private MiniMax _minMax = null;

        [TestInitialize]
        public void Initialize()
        {
            _minMax = new MiniMax();
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board board = new Board();
            _board = board.Players;

        }

        //Player 0 rolled 1
        //{"c-53":1,"c-49":1,"c-32":1,"c-0":1,"sq-1":0,"c-19":2,"sq-2":3,"Player rolling : ":1,"rolled":5}
        [TestMethod]
        public void TestDecisionMiniMax()
        {


            _board.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.Classic, 53, _board).MoveTo(CaseType.Classic, 53, _board);
            _board.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 49, _board).MoveTo(CaseType.Classic, 49, _board);
            _board.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 32, _board).MoveTo(CaseType.Classic, 32, _board);
            _board.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0, _board);

            _board.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19, _board);

            var currentState = new Node { State = _board, Roll = 2 };

            var nextState = _minMax.DecisionMiniMax(currentState, 3, 0);


            Assert.IsTrue(nextState.Item2 == 55, nextState.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxEndEntry()
        {

            _board.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.Classic, 54, _board).MoveTo(CaseType.Classic, 55, _board);
            _board.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 48, _board).MoveTo(CaseType.Classic, 49, _board);
            _board.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 31, _board).MoveTo(CaseType.Classic, 32, _board);
            _board.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0, _board);

            _board.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19, _board);

            var currentState = new Node { State = _board, Roll = 1 };

            var nextState = _minMax.DecisionMiniMax(currentState, 0, 0);

            Assert.IsTrue(nextState.Item2 == 1 && nextState.Item3 == CaseType.EndGame,
                nextState.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxEndGame()
        {

            _board.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.EndGame, 54, _board).MoveTo(CaseType.Classic, 55, _board);
            _board.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.EndGame, 4, _board).MoveTo(CaseType.EndGame, 5, _board);
            _board.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 31, _board).MoveTo(CaseType.Classic, 49, _board);
            _board.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0, _board);

            _board.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19, _board);

            var currentState = new Node { State = _board, Roll = 6 };

            var nextState = _minMax.DecisionMiniMax(currentState, 3, 0);

            nextState.Item1.MoveTo(nextState.Item3, nextState.Item2, currentState.State);

            var newPos = currentState.State.First(p => p.Id == 0);

            Assert.IsTrue(newPos.Won, newPos.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxEatThePawn()
        {

            _board.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.EndGame, 54, _board).MoveTo(CaseType.Classic, 55, _board);
            _board.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 31, _board).MoveTo(CaseType.Classic, 49, _board);

            _board.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 1, _board).MoveTo(CaseType.Classic, 1, _board);
            _board.First(p => p.Id == 1).Pawns[1].MoveTo(CaseType.Classic, 50, _board).MoveTo(CaseType.Classic, 51, _board);

            var currentState = new Node { State = _board, Roll = 2 };

            var nextState = _minMax.DecisionMiniMax(currentState, 3, 0);

            nextState.Item1.MoveTo(nextState.Item3, nextState.Item2, currentState.State);

            var newPos = currentState.State.First(p => p.Id == 0).Pawns[1];

            Assert.IsTrue(newPos.Position == 51 && newPos.Type == CaseType.Classic, newPos.ToString());

            var type = currentState.State.First(pl => pl.Id == 1).Pawns[1].Type;

            Assert.IsTrue(type == CaseType.Square, type.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxMoveFromBeginning()
        {

            _board.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.EndGame, 53, _board).MoveTo(CaseType.Classic, 54, _board);
            _board.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 0, _board);

            _board.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 14, _board);
            _board.First(p => p.Id == 1).Pawns[1].MoveTo(CaseType.Classic, 12, _board).MoveTo(CaseType.Classic, 13, _board);

            var currentState = new Node { State = _board, Roll = 4 };

            var nextState = _minMax.DecisionMiniMax(currentState, 3, 0);

            nextState.Item1.MoveTo(nextState.Item3, nextState.Item2, currentState.State);

            var newPos = currentState.State.First(p => p.Id == 0).Pawns[0];

            Assert.IsTrue(newPos.Position == 54 && newPos.Type == CaseType.Classic, newPos.ToString());

            newPos = currentState.State.First(pl => pl.Id == 0).Pawns[1];

            Assert.IsTrue(newPos.Position == 4 && newPos.Type == CaseType.Classic, newPos.Position.ToString());
        }
    }
}
