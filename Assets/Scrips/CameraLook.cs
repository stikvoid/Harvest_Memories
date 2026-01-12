using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Look Settings")]
    [Range(50f, 500f)] public float sensitivity = 150f;  // sensibilidad ajustable
    [Range(30f, 89f)] public float lookLimit = 80f;      // límite vertical

    private InputManager inputManager;
    private float verticalRotation = 0f; // rotación de la cámara (eje X)

    private Transform playerBody;

    private void Awake()
    {
        // Bloquear y ocultar el cursor para una experiencia FPS
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Buscar InputManager en el objeto padre
        inputManager = GetComponentInParent<InputManager>();
        playerBody = transform.parent;

        if (inputManager == null)
        {
            Debug.LogError("CameraLook ERROR: No se encontró el InputManager en el objeto padre.");
        }
    }

    private void LateUpdate()
    {
        if (inputManager == null) return;

        HandleLook();
    }

    private void HandleLook()
    {
        // Entrada de ratón
        Vector2 look = inputManager.lookInput * sensitivity * Time.deltaTime;

        // --- Rotación vertical (solo la cámara) ---
        verticalRotation -= look.y;  // invertir eje Y estándar (mirar arriba = mover mouse arriba)
        verticalRotation = Mathf.Clamp(verticalRotation, -lookLimit, lookLimit);

        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // --- Rotación horizontal (el jugador entero) ---
        // Esto rota el cuerpo (padre) en el eje Y, independiente de la cámara.
        playerBody.Rotate(Vector3.up * look.x);
    }
}
