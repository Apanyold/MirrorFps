using UnityEngine;
using Mirror;
using CodeBase.Services.Input;
using CodeBase.Infrastructure;
using CodeBase.Player;

namespace CodeBase.Client
{
    public class ClientPlayerController : NetworkBehaviour
    {
        public const float Epsilon = 0.001f;

        private InputService _inputService;
        [SerializeField]
        private Camera _camera;

        private PlayerController playerController;

        public bool lockMouseMovement = false;
        private float mouseSensitivity = 100f;
        private float xRotation = 0f;

        public void Init(PlayerController playerController)
        {
            this.playerController = playerController;
            _camera = playerController.playerCamera.GetComponent<Camera>();
        }

        private void Start()
        {
            _inputService = Game.inputService;
        }

        void Update()
        {
            if (_inputService.IsAttackButtonUp())
                Fire();

            if (_inputService.isMenuButtonDown())
                lockMouseMovement = !lockMouseMovement;

            MouseLook();

            Vector3 movementVector = Vector3.zero;
            var x = _inputService.Axis.x;
            var z = _inputService.Axis.y;

            if (_inputService.Axis.sqrMagnitude > Epsilon)
                movementVector = transform.right * x + transform.forward * z;

            movementVector += Physics.gravity;

            playerController.Move(movementVector);
        }

        public void MouseLook()
        {
            if (!lockMouseMovement)
            {
                Cursor.lockState = CursorLockMode.Locked;

                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                _camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                playerController.RotateGun(Quaternion.Euler(xRotation, 0f, 0f));
                playerController.RotatePlayer(Vector3.up * mouseX);
            }
            else
                Cursor.lockState = CursorLockMode.None;
        }

        private void OnDestroy()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        private void Fire()
        {
            playerController.Shoot();
        }
    }
}