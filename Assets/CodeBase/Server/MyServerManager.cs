using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using CodeBase;
using System;
using CodeBase.Player;

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
            NetworkServer.RegisterHandler<SeverMessage>(OnSendMessageServer);
        }

        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            Transform startPos = GetStartPosition();
            GameObject player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);


            var playerController = player.GetComponent<PlayerController>();
            playerController.ServerInit();

            // instantiating a "Player" prefab gives it the name "Player(clone)"
            // => appending the connectionId is WAY more useful for debugging!
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);

            NetworkServer.Spawn(player, conn);
            playerController.TargetInit();
        }

        public void OnSendMessageServer(NetworkConnection arg1, SeverMessage arg2)
        {
            Debug.Log(arg2.message);
        }

        public void OnSendMessageClient(SeverMessage arg2)
        {
            Debug.Log(arg2.message);
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);

            var message = new SeverMessage()
            {
                message = "connected to server"
            };

            conn.Send(message);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            NetworkClient.RegisterHandler<SeverMessage>(OnSendMessageClient);

            playerSpawner.CmdSpawnPlayer();
            var connection = NetworkClient.connection;
            var message = new SeverMessage()
            {
                message = "connected address: " + connection.address
            };

            connection.Send(message);
        }
    }

    public struct SeverMessage : NetworkMessage
    {
        public string message;
    }
}