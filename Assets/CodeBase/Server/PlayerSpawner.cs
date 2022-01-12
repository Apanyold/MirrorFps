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
        public void CmdSpawnPlayer()
        {
            networkManager.OnServerAddPlayer(NetworkClient.connection);
        }
    }
}