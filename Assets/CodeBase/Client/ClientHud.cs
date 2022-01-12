using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Client
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkManager))]
    public class ClientHud : MonoBehaviour
    {
        public int offsetX;
        public int offsetY;

        private NetworkManager _manager;

        void Awake()
        {
            _manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, 215, 9999));
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                    {
                        NetworkClient.AddPlayer();
                    }
                }
            }

            StopButtons();

            GUILayout.EndArea();
        }

        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    _manager.StartClient();
                }
                // This updates networkAddress every frame from the TextField
                _manager.networkAddress = GUILayout.TextField(_manager.networkAddress);
                GUILayout.EndHorizontal();
            }
            else
            {
                // Connecting
                GUILayout.Label($"Connecting to {_manager.networkAddress}..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    _manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // client only
            if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {_manager.networkAddress} via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            // stop client if client-only
            if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    _manager.StopClient();
                    //SceneLoader.LoadScene("LobbyScene")
                    //    .Then(_manager.StopClient);
                }
            }
        }
    }
}