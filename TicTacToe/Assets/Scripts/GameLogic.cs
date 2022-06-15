using UnityEngine;
using UnityEngine.UI;

namespace MimerUnity
{
    public class GameLogic : MonoBehaviour
    {
        private GameObject player1;
        private GameObject player2;
        private GameObject currentPlayer;
        private Image[] player1Images;
        private Image[] player2Images;
        private PlayerListener[] player1Listeners;
        private PlayerListener[] player2Listeners;

        void Start()
        {
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

            player1 = p1.gameObject;
            player2 = p2.gameObject;

            player1Images = player1.GetComponentsInChildren<Image>();
            if (player1Images == null || player1Images.Length == 0)
            {
                Debug.LogError("Found no images on player 1.");
            }
            player2Images = player2.GetComponentsInChildren<Image>();
            if (player2Images == null || player2Images.Length == 0)
            {
                Debug.LogError("Found no images on player 2.");
            }

            player1Listeners = player1.GetComponentsInChildren<PlayerListener>();
            if (player1Listeners == null || player1Listeners.Length == 0)
            {
                Debug.LogError("Found no player listeners on player 1.");
            }
            player2Listeners = player1.GetComponentsInChildren<PlayerListener>();
            if (player2Listeners == null || player2Listeners.Length == 0)
            {
                Debug.LogError("Found no player listeners on player 2.");
            }

            currentPlayer = player1;
            Player1SetActive(true);
            Player2SetActive(false);
        }

        public void ChangePlayer()
        {
            if (IsGameWon())
            {
                Debug.Log("Game won!");
            }
            else
            {
                if (currentPlayer == player1)
                {
                    currentPlayer = player2;
                    Player1SetActive(false);
                    Player2SetActive(true);
                }
                else if (currentPlayer == player2)
                {
                    currentPlayer = player1;
                    Player1SetActive(true);
                    Player2SetActive(false);
                }
                else
                {
                    Debug.LogError("Invalid current player");
                }
            }
        }

        private void Player1SetActive(bool active)
        {
            foreach (Image image in player1Images)
            {
                image.raycastTarget = active;
            }
        }

        private void Player2SetActive(bool active)
        {
            foreach (Image image in player2Images)
            {
                image.raycastTarget = active;
            }
        }

        private bool IsGameWon()
        {
            bool won = true;

            foreach (PlayerListener player in player1Listeners)
            {
                if (player.taken == false)
                {
                    won = false;
                }
            }

            return won;
        }
    }
}