using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MimerUnity
{
    public class PlayerListener : MonoBehaviour, IPointerDownHandler
    {
        private GameLogic gameLogic;
        private Mask mask;
        private PlayerListener otherPlayersCorresponding;

        public bool Taken { get; set; }

        void Start()
        {
            gameLogic = gameObject.GetComponentInParent<GameLogic>();
            if (gameLogic == null)
            {
                Debug.LogError("Failed to find game logic.");
            }
            
            mask = gameObject.GetComponent<Mask>();
            if (mask == null)
            {
                Debug.LogError("Failed to find graphics mask.");
            }

            GetOtherPlayersCorresponding();

            mask.showMaskGraphic = false;
            Taken = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Taken)
            {
                mask.showMaskGraphic = true;
                gameLogic.ChangePlayer();
                Taken = true;
                otherPlayersCorresponding.Taken = true;
            }
        }

        private void GetOtherPlayersCorresponding()
        {
            string myName = gameObject.name;
            Transform myPlayerTransform = transform.parent;
            if (myPlayerTransform == null)
            {
                Debug.LogError("PlayerListener has no parent.");
            }

            string myPlayerName = myPlayerTransform.gameObject.name;
            string otherPlayerName = myPlayerName == "Player 2" ? "Player 1" : "Player 2";
            Transform board = myPlayerTransform.parent;
            if (board == null)
            {
                Debug.LogError("My player has no parent.");
            }

            Transform otherPlayerTransform = board.Find(otherPlayerName);
            if (otherPlayerTransform == null)
            {
                Debug.LogError("Board has no object named " + otherPlayerName);
            }

            Transform otherPlayersCorrespondingTransform = otherPlayerTransform.Find(myName);
            if (otherPlayersCorrespondingTransform == null)
            {
                Debug.LogError("Other player has no child called " + myName);
            }

            otherPlayersCorresponding = otherPlayersCorrespondingTransform.GetComponent<PlayerListener>();
            if (otherPlayersCorresponding == null)
            {
                Debug.LogError("Other player's corresponding child has no PlayerListener");
            }
        }
    }
}