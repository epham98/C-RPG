using System;
using Engine.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//runs automated test of gamesession and its objects

namespace TestEngine.ViewModels
{
    [TestClass]
    public class TestGameSession
    {
        [TestMethod]
        public void TestCreateGameSession()
        {
            GameSession gameSession = new GameSession();

            Assert.IsNotNull(gameSession.currentPlayer);
            Assert.AreEqual("Town square", gameSession.currentLocation.name);
        }

        [TestMethod]
        public void TestPlayerMoveHomeAndFullHeal()
        {
            GameSession gameSession = new GameSession();

            gameSession.currentPlayer.takeDamage(999);

            Assert.AreEqual("Home", gameSession.currentLocation.name);
            Assert.AreEqual(gameSession.currentPlayer.level * 10, gameSession.currentPlayer.currentHP);
        }
    }
}
