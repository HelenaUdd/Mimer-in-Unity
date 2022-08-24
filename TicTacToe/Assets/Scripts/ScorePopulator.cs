using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace MimerUnity
{
    public class ScorePopulator : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI dateColumn;
        private TMPro.TextMeshProUGUI playerColumn;
        private TMPro.TextMeshProUGUI movesColumn;
        private TMPro.TextMeshProUGUI timeColumn;

        private Task<System.Data.Common.DbDataReader> selectTask = null;

        public void Start()
        {
            if (IsFindingComponents())
            {
                UpdateHighscoreTable();
            }
        }

        private bool IsFindingComponents()
        {
            TMPro.TextMeshProUGUI[] texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();

            if (texts != null && texts.Length > 0)
            {
                foreach (TMPro.TextMeshProUGUI text in texts)
                {
                    if (text.gameObject.name.Equals("Date column"))
                    {
                        dateColumn = text;
                    }
                    else if (text.gameObject.name.Equals("Player column"))
                    {
                        playerColumn = text;
                    }
                    else if (text.gameObject.name.Equals("Moves column"))
                    {
                        movesColumn = text;
                    }
                    else if (text.gameObject.name.Equals("Time column"))
                    {
                        timeColumn = text;
                    }
                }

                return true;
            }
            else
            {
                Debug.LogError("Failed to find text meshes to populate.");
                return false;
            }
        }

        public void UpdateHighscoreTable()
        {
            DatabaseCommunicator.Instance.Open();
            selectTask = DatabaseCommunicator.Instance.GetHighscoresAsync();
        }

        private void Update()
        {
            if (selectTask != null)
            {
                if (selectTask.IsCompleted)
                {
                    List<DatabaseCommunicator.Highscore> scores =
                        DatabaseCommunicator.Instance.GetHighScoresFromDbReader(selectTask.Result);
                    Debug.Log($"Fetched {scores.Count} highscore(s) from database.");
                    PopulateHighscoreTable(scores);
                    selectTask = null;
                }
            }
        }

        private void PopulateHighscoreTable(List<DatabaseCommunicator.Highscore> scores)
        {
            var dateBuilder = new StringBuilder();
            var playerBuilder = new StringBuilder();
            var movesBuilder = new StringBuilder();
            var timeBuilder = new StringBuilder();

            foreach (DatabaseCommunicator.Highscore score in scores)
            {
                dateBuilder.AppendLine(score.occurrance.ToString());
                playerBuilder.AppendLine(score.player.ToString());
                movesBuilder.AppendLine(score.moves.ToString());
                timeBuilder.Append(score.time_spent.TotalSeconds.ToString());
                timeBuilder.AppendLine(" s");
            }

            if (dateColumn != null)
                dateColumn.text = dateBuilder.ToString();
            if (playerColumn != null)
                playerColumn.text = playerBuilder.ToString();
            if (movesColumn != null)
                movesColumn.text = movesBuilder.ToString();
            if (timeColumn != null)
                timeColumn.text = timeBuilder.ToString();
        }
    }
}