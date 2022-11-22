using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class ClientManager : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
                StartButtons();
            else
                StatusLabels();

            GUILayout.EndArea();
        }

        private void Update()
        {
            SubmitNewPosition();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewPosition()
        {
            /**
            Debug.Log("SubmitNewPosition!");

            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
            {
                Debug.Log("Server move is called!");

                foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                    NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<ClientMovement>().Move();
            }
            else
            {
                if (NetworkManager.Singleton.SpawnManager != null)
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    if (playerObject != null)
                    {
                        var player = playerObject.GetComponent<ClientMovement>();
                        player.Move();
                    }
                }
                Debug.Log("Client move is called!");
            }
            /**/
        }
    }
}
