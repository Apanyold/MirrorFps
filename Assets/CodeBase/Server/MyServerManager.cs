using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CodeBase;

namespace CodeBase.Server
{
    public class MyServerManager : NetworkManager
    {
        public PlayerSpawner playerSpawner;

        public override void Start()
        {
            base.Start();
            playerSpawner.Init(this);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Transform startPos = GetStartPosition();
            GameObject player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);


            var playerController = player.GetComponent<PlayerController>();

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);

            NetworkServer.Spawn(player, conn);
            playerController.Init();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            playerSpawner.CmdSpawnPlayer();
        }
    }
}