using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MimerUnity
{
    public class PlayerMarker : MonoBehaviour, IPointerDownHandler
    {
        private GameLogic gameLogic;
        private Mask mask;
        private PlayerMarker otherPlayersMarker;

        public bool taken = false;
        public bool takenByMe = false;

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

            GetOtherPlayersMarker();

            mask.showMaskGraphic = false;
            taken = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!gameLogic.GameOver && !taken)
            {
                taken = takenByMe = true;
                otherPlayersMarker.taken = true;
                mask.showMaskGraphic = true;
                gameLogic.ChangePlayer();
            }
        }

        private void GetOtherPlayersMarker()
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

            Transform otherPlayersMarkerTransform = otherPlayerTransform.Find(myName);
            if (otherPlayersMarkerTransform == null)
            {
                Debug.LogError("Other player has no child called " + myName);
            }

            otherPlayersMarker = otherPlayersMarkerTransform.GetComponent<PlayerMarker>();
            if (otherPlayersMarker == null)
            {
                Debug.LogError("Other player's corresponding child has no PlayerMarker");
            }
        }
    }
}