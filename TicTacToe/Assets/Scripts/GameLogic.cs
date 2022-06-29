using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace MimerUnity
{
    public class GameLogic : MonoBehaviour
    {
        public bool GameOngoing { get; private set; }
        public bool GameOver { get; private set; }
        
        private class Player
        {
            public GameObject gameObject;
            public Image[] images;
            public Dictionary<string, PlayerMarker> playerMarkers;
            public short nrOfMoves;
        }

        private Player[] players = new Player[2];
        private Player currentPlayer;
        
        private GameObject startNewGameButton;
        private ScorePopulator scorePopulator;
        private TMPro.TextMeshProUGUI gameTimeText;

        private System.Diagnostics.Stopwatch stopwatch = new();

        private void Start()
        {
            GetPlayers();
            GetHelperObjects();

            currentPlayer = null;
            PlayerReset(0);
            PlayerReset(1);

            GameOngoing = false;
            GameOver = false;
        }

        private void Update()
        {
            float milliseconds = stopwatch.ElapsedMilliseconds;
            float seconds = milliseconds / 1000;
            gameTimeText.text = seconds.ToString("0.00") + " s";
        }

        public void StartNewGame()
        {
            startNewGameButton.SetActive(false);
            PlayerReset(0);
            PlayerReset(1);

            currentPlayer = players[0];
            PlayerSetActive(0, true);
            PlayerSetActive(1, false);
            GameOngoing = true;
            GameOver = false;

            stopwatch.Reset();
            stopwatch.Start();
        }

        public void ChangePlayer()
        {
            if (GameOngoing)
            {
                currentPlayer.nrOfMoves++;

                if (HasPlayerWon(0))
                {
                    stopwatch.Stop();
                    Debug.Log("Player 1 has won!");
                    AddHighscore(1, currentPlayer.nrOfMoves);
                    GameOver = true;
                    startNewGameButton.SetActive(true);
                }
                else if (HasPlayerWon(1))
                {
                    stopwatch.Stop();
                    Debug.Log("Player 2 has won!");
                    AddHighscore(2, currentPlayer.nrOfMoves);
                    GameOver = true;
                    startNewGameButton.SetActive(true);
                }
                else if (IsGameOver())
                {
                    stopwatch.Stop();
                    Debug.Log("Game is over, no one won.");
                    GameOver = true;
                    startNewGameButton.SetActive(true);
                }
                else
                {
                    if (currentPlayer == players[0])
                    {
                        currentPlayer = players[1];
                        PlayerSetActive(0, false);
                        PlayerSetActive(1, true);
                    }
                    else if (currentPlayer == players[1])
                    {
                        currentPlayer = players[0];
                        PlayerSetActive(0, true);
                        PlayerSetActive(1, false);
                    }
                    else
                    {
                        Debug.LogError($"Invalid current player {(currentPlayer.gameObject != null ? currentPlayer.gameObject.name : currentPlayer)}");
                    }
                }
            }
        }

        private void GetPlayers()
        {
            players[0] = new Player();
            players[1] = new Player();
            
            Transform p1 = transform.Find("Board/Player 1");
            Transform p2 = transform.Find("Board/Player 2");
            
            if (p1 == null)
            {
                Debug.LogError("Game object Player 1 not found");
            }
            if (p2 == null)
            {
                Debug.LogError("Game object Player 2 not found");
            }

            players[0].gameObject = p1.gameObject;
            players[1].gameObject = p2.gameObject;

            players[0].images = players[0].gameObject.GetComponentsInChildren<Image>();
            if (players[0].images == null || players[0].images.Length == 0)
            {
                Debug.LogError("Found no images on player 1.");
            }
            players[1].images = players[1].gameObject.GetComponentsInChildren<Image>();
            if (players[1].images == null || players[1].images.Length == 0)
            {
                Debug.LogError("Found no images on player 2.");
            }

            PlayerMarker[] player1Markers = players[0].gameObject.GetComponentsInChildren<PlayerMarker>();
            if (player1Markers == null || player1Markers.Length == 0)
            {
                Debug.LogError("Found no player markers on player 1.");
            }
            PlayerMarker[] player2Markers = players[1].gameObject.GetComponentsInChildren<PlayerMarker>();
            if (player2Markers == null || player2Markers.Length == 0)
            {
                Debug.LogError("Found no player markers on player 2.");
            }

            players[0].playerMarkers = new Dictionary<string, PlayerMarker>(player1Markers.Length);
            players[1].playerMarkers = new Dictionary<string, PlayerMarker>(player2Markers.Length);
            foreach (PlayerMarker marker in player1Markers)
            {
                players[0].playerMarkers[marker.gameObject.name] = marker;
            }
            foreach (PlayerMarker marker in player2Markers)
            {
                players[1].playerMarkers[marker.gameObject.name] = marker;
            }
        }

        private void GetHelperObjects()
        {
            Transform buttonTransform = transform.Find("Start game button");
            if (buttonTransform == null)
            {
                Debug.LogError("Failed to find start new game button");
            }
            startNewGameButton = buttonTransform.gameObject;

            Transform highscoresTransform = transform.parent.Find("Highscores");
            Transform scorePopulatorTransform = highscoresTransform.Find("ScorePopulator");
            if (scorePopulatorTransform == null)
            {
                Debug.LogError("Failed to find score populator transform");
            }
            scorePopulator = scorePopulatorTransform.GetComponent<ScorePopulator>();
            if (scorePopulator == null)
            {
                Debug.LogError("Failed to find score populator");
            }

            Transform gameTimeTextTransform = transform.Find("Game time");
            if (gameTimeTextTransform == null)
            {
                Debug.LogError("Failed to find game time text transform");
            }
            gameTimeText = gameTimeTextTransform.GetComponent<TMPro.TextMeshProUGUI>();
            if (gameTimeText == null)
            {
                Debug.LogError("Failed to find game time text component");
            }
        }

        private void PlayerSetActive(int index, bool active)
        {
            foreach (Image image in players[index].images)
            {
                image.raycastTarget = active;
            }
        }

        private void PlayerReset(int index)
        {
            PlayerSetActive(index, false);
            players[index].nrOfMoves = 0;

            foreach (KeyValuePair<string, PlayerMarker> marker in players[index].playerMarkers)
            {
                marker.Value.Unset();
            }
        }

        private bool IsGameOver()
        {
            bool over = true;

            foreach (KeyValuePair<string, PlayerMarker> marker in players[0].playerMarkers)
            {
                if (marker.Value.taken == false)
                {
                    over = false;
                }
            }

            return over;
        }

        private bool HasPlayerWon(int playerIndex)
        {
            bool win = false;

            if (players[playerIndex].playerMarkers["A1"].takenByMe)
            {
                if (players[playerIndex].playerMarkers["B1"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["C1"].takenByMe)
                    {
                        /*
                        *  [x][ ][ ]
                        *  [x][ ][ ]
                        *  [x][ ][ ]
                        */
                        win = true;
                    }
                }
                else if (players[playerIndex].playerMarkers["A2"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["A3"].takenByMe)
                    {
                        /*
                        *  [x][x][x]
                        *  [ ][ ][ ]
                        *  [ ][ ][ ]
                        */
                        win = true;
                    }
                }
                else if (players[playerIndex].playerMarkers["B2"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["C3"].takenByMe)
                    {
                        /*
                        *  [x][ ][ ]
                        *  [ ][x][ ]
                        *  [ ][ ][x]
                        */
                        win = true;
                    }
                }
            }

            if (!win && players[playerIndex].playerMarkers["A2"].takenByMe)
            {
                if (players[playerIndex].playerMarkers["B2"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["C2"].takenByMe)
                    {
                        /*
                        *  [ ][x][ ]
                        *  [ ][x][ ]
                        *  [ ][x][ ]
                        */
                        win = true;
                    }
                }
            }

            if (!win && players[playerIndex].playerMarkers["A3"].takenByMe)
            {
                if (players[playerIndex].playerMarkers["B3"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["C3"].takenByMe)
                    {
                        /*
                        *  [ ][ ][x]
                        *  [ ][ ][x]
                        *  [ ][ ][x]
                        */
                        win = true;
                    }
                }
                else if (players[playerIndex].playerMarkers["B2"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["C1"].takenByMe)
                    {
                        /*
                        *  [ ][ ][x]
                        *  [ ][x][ ]
                        *  [x][ ][ ]
                        */
                        win = true;
                    }
                }
            }

            if (!win && players[playerIndex].playerMarkers["B1"].takenByMe)
            {
                if (players[playerIndex].playerMarkers["B2"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["B3"].takenByMe)
                    {
                        /*
                        *  [ ][ ][ ]
                        *  [x][x][x]
                        *  [ ][ ][ ]
                        */
                        win = true;
                    }
                }
            }

            if (!win && players[playerIndex].playerMarkers["C1"].takenByMe)
            {
                if (players[playerIndex].playerMarkers["C2"].takenByMe)
                {
                    if (players[playerIndex].playerMarkers["C3"].takenByMe)
                    {
                        /*
                        *  [ ][ ][ ]
                        *  [ ][ ][ ]
                        *  [x][x][x]
                        */
                        win = true;
                    }
                }
            }

            return win;
        }

        private void AddHighscore(short player, short moves)
        {
            var score = new DatabaseCommunicator.Highscore();
            score.occurrance = DateTime.Now;
            score.player = player;
            score.moves = moves;
            score.time_spent = stopwatch.Elapsed;
            DatabaseCommunicator.Instance.AddHighscore(score);
            scorePopulator.PopulateHighscoreTable();
        }
    }
}