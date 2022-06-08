using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace MimerUnity
{
    public class ScorePopulator : MonoBehaviour
    {
        DatabaseCommunicator database;
        TMPro.TextMeshProUGUI dateColumn;
        TMPro.TextMeshProUGUI playerColumn;
        TMPro.TextMeshProUGUI movesColumn;
        TMPro.TextMeshProUGUI timeColumn;

        void Start()
        {
            if (IsFindingComponents())
            {
                PopulateHighscoreTable();
            }
        }

        private bool IsFindingComponents()
        {
            database = GetComponentInChildren<DatabaseCommunicator>();
            TMPro.TextMeshProUGUI[] texts = GetComponentsInChildren<TMPro.TextMeshProUGUI>();

            if (database != null)
            {
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
                    Debug.Log("Failed to find text meshes to populate.");
                    return false;
                }
            }
            else
            {
                Debug.Log("Failed to find DatabaseCommunicator.");
                return false;
            }
        }

        private void PopulateHighscoreTable()
        {
            List<DatabaseCommunicator.Highscore> scores = database.GetHighscores();

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