using UnityEngine;
using CodeBase.Services.Input;
using System;
using Mirror;

namespace CodeBase.Infrastructure
{
    public class Game: MonoBehaviour
    {
        public static InputService inputService;
        public static NetworkManager networkManager;

        private void Awake()
        {
            inputService = new StandaloneInputService();

            networkManager = FindObjectOfType<NetworkManager>();
        }

        public static Transform GetStartPosition() => networkManager.GetStartPosition();
    }
}