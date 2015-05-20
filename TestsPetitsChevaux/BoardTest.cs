using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetitsChevaux.Game;

namespace TestsPetitsChevaux
{
    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void TestNormalize()
        {
            for (int i = 0; i < 10; i++)
            {
                var r = Board.Normalize(i, 5);
                Assert.IsTrue(r < 5 && r >= 0, r.ToString());
            }

            for (int i = 1; i <= 10; i++)
            {
                var r = Board.Normalize(i, 5, 1);
                Assert.IsTrue(r >= 1 && r < 6, r.ToString());
            }

            Assert.AreEqual(55, Board.Normalize(-1));

        }
    }
}
