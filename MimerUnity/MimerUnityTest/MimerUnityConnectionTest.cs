using Mimer.Data.Client;
using NUnit.Framework;
using System;

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

        [Test]
        public void ExecuteCommandTest()
        {
            connection.Open("UnityDemo", "tictactoe", "tictactoe");

            connection.ExecuteNonQueryCommand(
                "insert into highscores values (timestamp'2022-06-06 15:00:00', 2, 10, interval '12.3' second)");
            MimerDataReader reader = connection.ExecuteReaderCommand(
                "select occurrance, player, moves, time_spent from highscores where occurrance = timestamp'2022-06-06 15:00:00'");

            DateTime occurrance;
            short player;
            short moves;
            TimeSpan time_spent;

            if (reader.Read())
            {
                occurrance = reader.GetDateTime(0);
                player = reader.GetInt16(1);
                moves = reader.GetInt16(2);
                time_spent = reader.GetTimeSpan(3);

                try
                {
                    Assert.AreEqual(new DateTime(2022, 6, 6, 15, 00, 0), occurrance);
                    Assert.AreEqual(2, player);
                    Assert.AreEqual(10, moves);
                    Assert.IsTrue(12.29 < time_spent.TotalSeconds);
                    Assert.IsTrue(12.31 > time_spent.TotalSeconds);
                }
                catch (AssertionException)
                {
                    connection.ExecuteNonQueryCommand(
                        "delete from highscores where occurrance = timestamp'2022-06-06 15:00:00'");
                    connection.Close();

                    throw;
                }
            }

            connection.ExecuteNonQueryCommand(
                "delete from highscores where occurrance = timestamp'2022-06-06 15:00:00'");
            connection.Close();
        }
    }
}