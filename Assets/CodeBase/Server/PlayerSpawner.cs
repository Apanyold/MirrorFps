using Mirror;
using UnityEngine;

namespace CodeBase.Server
{
    public class PlayerSpawner : NetworkBehaviour
    {
        private NetworkManager networkManager;

        public void Init(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        [Command]
        public void CmdSpawnPlayer(NetworkConnectionToClient sender = null)
        {
            networkManager.OnServerAddPlayer(NetworkClient.connection);
        }
    }
}