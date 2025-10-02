using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cam; // ← Cámara asignada desde el Inspector

    private CharacterController controller;
    private float verticalRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimiento
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Rotación horizontal (Jugador)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical (Cámara)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cam.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}