using UnityEngine;
using Mimer.Data.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MimerUnity
{
    public class DatabaseCommunicator : MonoBehaviour
    {
        private MimerConnection connection;

        public struct Highscore
        {
            public DateTime occurrance;
            public short player;
            public short moves;
            public TimeSpan time_spent;
        }

        public List<Highscore> GetHighscores()
        {
            short elementsToFetch = 5;

            Open("UnityDemo", "tictactoe", "tictactoe");

            List<Highscore> list = new List<Highscore>(elementsToFetch);

            var commandBuilder = new StringBuilder();
            commandBuilder.Append("SELECT occurrance, player, moves, time_spent FROM highscores");
            commandBuilder.Append(" ORDER BY moves, time_spent FETCH FIRST ");
            commandBuilder.Append(elementsToFetch);

            MimerCommand selectCommand = new MimerCommand(commandBuilder.ToString(), connection);
            MimerDataReader reader = selectCommand.ExecuteReader();

            while (reader.Read())
            {
                Highscore highscore = new Highscore()
                {
                    occurrance = reader.GetDateTime(0),
                    player = reader.GetInt16(1),
                    moves = reader.GetInt16(2),
                    time_spent = reader.GetTimeSpan(3)
                };

                list.Add(highscore);
            }

            Debug.Log($"Fetched {list.Count} highscore(s) from database.");
            connection.Close();

            return list;
        }

        private void Open(string database, string username, string password)
        {
            var connectionString = new MimerConnectionStringBuilder();
            connectionString.Add("Database", database);
            connectionString.Add("User ID", username);
            connectionString.Add("Password", password);

            if (connection != null)
            {
                connection.Close();
                connection = null;
            }

            connection = new MimerConnection(connectionString.ToString());
            connection.Open();
        }
    }
}

