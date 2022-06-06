using NUnit.Framework;

namespace MimerUnity.Test
{
    public class MimerUnityConnectionTest
    {
        private MimerUnityConnection connection;

        [SetUp]
        public void Setup()
        {
            connection = new MimerUnityConnection();
        }

        [Test]
        public void ConnectionTest()
        {
            connection.Open("UnityDemo", "tictactoe", "tictactoe");
            string currentUser = connection.CurrentUser;
            connection.Close();

            Assert.AreEqual("TICTACTOE", currentUser);
        }
    }
}