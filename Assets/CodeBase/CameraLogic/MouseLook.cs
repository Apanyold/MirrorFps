using UnityEngine;
using Mirror;

public class MouseLook : MonoBehaviour
{
    public bool lockMouseMovement = false;

    [SerializeField]
    private Transform playerTransform;
    private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private bool isLocal;

    void Start()
    {
        isLocal = playerTransform.GetComponent<NetworkBehaviour>().isLocalPlayer;
    }

    void Update()
    {
        if (!isLocal)
            return;

        if (!lockMouseMovement)
        {
            Cursor.lockState = CursorLockMode.Locked;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerTransform.Rotate(Vector3.up * mouseX);
        }
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
