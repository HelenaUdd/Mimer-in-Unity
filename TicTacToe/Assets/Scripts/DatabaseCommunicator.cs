using UnityEngine;
using MimerUnity;
using Mimer.Data.Client;

namespace MimerUnity
{
    public class DatabaseCommunicator : MonoBehaviour
    {
        MimerUnityConnection connection = new MimerUnityConnection();
        
        public void Start()
        {
            connection.Open("UnityDemo", "tictactoe", "tictactoe");
            string currentUser = connection.CurrentUser;

            Debug.Log(currentUser);
        }
        
        public void OnDestroy()
        {
            connection.Close();
        }
    }
}

