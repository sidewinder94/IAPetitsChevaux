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

            Board.Players.First(p => p.Id == 0).Pawns[0].MoveTo(CaseType.Classic, 53);
            Board.Players.First(p => p.Id == 0).Pawns[1].MoveTo(CaseType.Classic, 49);
            Board.Players.First(p => p.Id == 0).Pawns[2].MoveTo(CaseType.Classic, 32);
            Board.Players.First(p => p.Id == 0).Pawns[3].MoveTo(CaseType.Classic, 0);

            Board.Players.First(p => p.Id == 1).Pawns[0].MoveTo(CaseType.Classic, 19);

            var currentState = new Node { State = Board.Players, Roll = 1 };

            Node nextState = MiniMax.DecisionMiniMax(currentState, 3, 0);


            var newPos = nextState.State.First(p => p.Id == 0).Pawns[0].Position;

            Assert.IsTrue(newPos == 54, newPos.ToString());

        }
    }
}
