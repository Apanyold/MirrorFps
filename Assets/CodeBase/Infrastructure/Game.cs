using UnityEngine;
using CodeBase.Services.Input;

namespace CodeBase.Infrastructure
{
    public class Game: MonoBehaviour
    {
        public static InputService _inputService;

        private void Awake()
        {
            _inputService = new StandaloneInputService();
        }
    }
}