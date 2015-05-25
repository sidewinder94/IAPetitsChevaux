using System;
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
        //Player 0 rolled 1
        //{"c-53":1,"c-49":1,"c-32":1,"c-0":1,"sq-1":0,"c-19":2,"sq-2":3,"Player rolling : ":1,"rolled":5}
        [TestMethod]
        public void TestDecisionMiniMax()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board.GeneratePlayers();

            Board.Players.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.Classic, 53).MoveTo(CaseType.Classic, 53);
            Board.Players.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 49).MoveTo(CaseType.Classic, 49);
            Board.Players.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 32).MoveTo(CaseType.Classic, 32);
            Board.Players.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0);

            Board.Players.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19);

            var currentState = new Node { State = Board.Players, Roll = 2 };

            Node nextState = MiniMax.DecisionMiniMax(currentState, 3, 0);


            var newPos = nextState.State.First(p => p.Id == 0).Pawns[0].Position;

            Assert.IsTrue(newPos == 55, newPos.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxEndEntry()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board.GeneratePlayers();

            Board.Players.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.Classic, 54).MoveTo(CaseType.Classic, 55);
            Board.Players.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 48).MoveTo(CaseType.Classic, 49);
            Board.Players.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 31).MoveTo(CaseType.Classic, 32);
            Board.Players.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0);

            Board.Players.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19);

            var currentState = new Node { State = Board.Players, Roll = 1 };

            Node nextState = MiniMax.DecisionMiniMax(currentState, 3, 0);


            var newPos = nextState.State.First(p => p.Id == 0).Pawns[0];

            Assert.IsTrue(newPos.Position == 1 && newPos.Type == CaseType.EndGame,
                newPos.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxEndGame()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board.GeneratePlayers();

            Board.Players.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.EndGame, 54).MoveTo(CaseType.Classic, 55);
            Board.Players.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.EndGame, 4).MoveTo(CaseType.EndGame, 5);
            Board.Players.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 31).MoveTo(CaseType.Classic, 49);
            Board.Players.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0);

            Board.Players.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19);

            var currentState = new Node { State = Board.Players, Roll = 6 };

            Node nextState = MiniMax.DecisionMiniMax(currentState, 3, 0);

            var newPos = nextState.State.First(p => p.Id == 0);

            Assert.IsTrue(newPos.Won, newPos.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxEatThePawn()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board.GeneratePlayers();

            Board.Players.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.EndGame, 54).MoveTo(CaseType.Classic, 55);
            Board.Players.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 31).MoveTo(CaseType.Classic, 49);

            Board.Players.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 1).MoveTo(CaseType.Classic, 1);
            Board.Players.First(p => p.Id == 1).Pawns[1].MoveTo(CaseType.Classic, 50).MoveTo(CaseType.Classic, 51);

            var currentState = new Node { State = Board.Players, Roll = 2 };

            Node nextState = MiniMax.DecisionMiniMax(currentState, 3, 0);

            var newPos = nextState.State.First(p => p.Id == 0).Pawns[1];

            Assert.IsTrue(newPos.Position == 51 && newPos.Type == CaseType.Classic, newPos.ToString());

            var type = nextState.State.First(pl => pl.Id == 1).Pawns[1].Type;

            Assert.IsTrue(type == CaseType.Square, type.ToString());
        }

        [TestMethod]
        public void TestDecisionMiniMaxMoveFromBeginning()
        {
            Board.PawnsPerPlayer = 4;
            Board.PlayerNumber = 2;
            Board.GeneratePlayers();

            Board.Players.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.EndGame, 53).MoveTo(CaseType.Classic, 54);
            Board.Players.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 0);

            Board.Players.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 14);
            Board.Players.First(p => p.Id == 1).Pawns[1].MoveTo(CaseType.Classic, 12).MoveTo(CaseType.Classic, 13);

            var currentState = new Node { State = Board.Players, Roll = 4 };

            Node nextState = MiniMax.DecisionMiniMax(currentState, 3, 0);

            var newPos = nextState.State.First(p => p.Id == 0).Pawns[0];

            Assert.IsTrue(newPos.Position == 54 && newPos.Type == CaseType.Classic, newPos.ToString());

            newPos = nextState.State.First(pl => pl.Id == 0).Pawns[1];

            Assert.IsTrue(newPos.Position == 4 && newPos.Type == CaseType.Classic, newPos.Position.ToString());
        }
    }
}
