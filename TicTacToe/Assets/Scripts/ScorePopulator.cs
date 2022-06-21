using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace MimerUnity
{
    public class ScorePopulator : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI dateColumn;
        private TMPro.TextMeshProUGUI playerColumn;
        private TMPro.TextMeshProUGUI movesColumn;
        private TMPro.TextMeshProUGUI timeColumn;

        public void Start()
        {
            if (IsFindingComponents())
            {
                PopulateHighscoreTable();
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

        private void PopulateHighscoreTable()
        {
            List<DatabaseCommunicator.Highscore> scores = DatabaseCommunicator.Instance.GetHighscores();

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