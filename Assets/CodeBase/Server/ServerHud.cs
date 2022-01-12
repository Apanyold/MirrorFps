using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace CodeBase.Server
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkManager))]
    public class ServerHud : MonoBehaviour
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
                // This updates networkAddress every frame from the TextField
                GUILayout.BeginHorizontal();
                _manager.networkAddress = GUILayout.TextField(_manager.networkAddress);
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Server Only")) 
                {
                    //SceneLoader.LoadScene("GameScene")
                    //  .Then(_manager.StartServer);
                    _manager.StartServer();
                } 
            }
        }

        void StatusLabels()
        {
            // server only
            if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
        }

        void StopButtons()
        {
            if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    //SceneLoader.LoadScene("ServerScene")
                    //    .Then(_manager.StopServer);
                    _manager.StopServer();
                }
            }
        }
    }
}