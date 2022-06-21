using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace MimerUnity
{
    public class GameLogic : MonoBehaviour
    {
        public bool GameOver { get; private set; }
        
        private struct Player
        {
            public GameObject gameObject;
            public Image[] images;
            public Dictionary<string, PlayerMarker> playerMarkers;
        }

        private Player[] players = new Player[2];
        private Player currentPlayer;

        void Start()
        {
            GetPlayers();

            currentPlayer = players[0];
            PlayerSetActive(0, true);
            PlayerSetActive(1, false);

            GameOver = false;
        }

        public void ChangePlayer()
        {
            if (HasPlayerWon(0))
            {
                Debug.Log("Player 1 has won!");
                AddHighscore(1);
                GameOver = true;
            }
            else if (HasPlayerWon(1))
            {
                Debug.Log("Player 2 has won!");
                AddHighscore(2);
                GameOver = true;
            }
            else if (IsGameOver())
            {
                Debug.Log("Game is over, no one won.");
                GameOver = true;
            }
            else
            {
                if (currentPlayer.Equals(players[0]))
                {
                    currentPlayer = players[1];
                    PlayerSetActive(0, false);
                    PlayerSetActive(1, true);
                }
                else if (currentPlayer.Equals(players[1]))
                {
                    currentPlayer = players[0];
                    PlayerSetActive(0, true);
                    PlayerSetActive(1, false);
                }
                else
                {
                    Debug.LogError("Invalid current player");
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

        private void PlayerSetActive(int index, bool active)
        {
            foreach (Image image in players[index].images)
            {
                image.raycastTarget = active;
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

        private void AddHighscore(short player)
        {
            var score = new DatabaseCommunicator.Highscore();
            score.occurrance = DateTime.Now;
            score.player = player;
            DatabaseCommunicator.Instance.AddHighscore(score);
        }
    }
}