using UnityEngine;
using Mimer.Data.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MimerUnity
{
    public class DatabaseCommunicator : MonoBehaviour
    {
        private MimerConnection connection;

        public static DatabaseCommunicator Instance
        {
            get;
            private set;
        }

        public struct Highscore
        {
            public DateTime occurrance;
            public short player;
            public short moves;
            public TimeSpan time_spent;
        }

        public List<Highscore> GetHighscores()
        {
            Open("UnityDemo", "tictactoe", "tictactoe");

            Task<System.Data.Common.DbDataReader> selectTask = GetHighscoresAsync();
            selectTask.Wait();

            MimerDataReader reader = (MimerDataReader)selectTask.Result;
            List<Highscore> list = new List<Highscore>();
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
            Close();

            return list;
        }

        public Task<System.Data.Common.DbDataReader> GetHighscoresAsync()
        {
            short elementsToFetch = 5;

            var commandBuilder = new StringBuilder();
            commandBuilder.Append("SELECT occurrance, player, moves, time_spent FROM highscores");
            commandBuilder.Append(" ORDER BY moves, time_spent FETCH FIRST ");
            commandBuilder.Append(elementsToFetch);

            MimerCommand selectCommand = new (commandBuilder.ToString(), connection);
            Task<System.Data.Common.DbDataReader> selectTask = selectCommand.ExecuteReaderAsync();

            return selectTask;
        }

        public void AddHighscore(Highscore highscore)
        {
            Open("UnityDemo", "tictactoe", "tictactoe");

            Task<int> insertTask = AddHighscoreAsync(highscore);
            insertTask.Wait();

            Debug.Log($"Inserted a highscore into database.");
            Close();
        }

        public Task<int> AddHighscoreAsync(Highscore highscore)
        {
            MimerCommand insertCommand = 
                new MimerCommand("INSERT INTO highscores VALUES (:occurrance, :player, :moves, :time_spent)", 
                connection);
            insertCommand.Parameters.Add(new MimerParameter(":occurrance", highscore.occurrance));
            insertCommand.Parameters.Add(new MimerParameter(":player", highscore.player));
            insertCommand.Parameters.Add(new MimerParameter(":moves", highscore.moves));
            insertCommand.Parameters.Add(new MimerParameter(":time_spent", highscore.time_spent));
            
            Task<int> insertTask = insertCommand.ExecuteNonQueryAsync();
            insertCommand.Parameters.Clear();

            return insertTask;
        }

        public void Open(string database, string username, string password)
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

        public void Close()
        {
            connection.Close();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}

