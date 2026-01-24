using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GravedadGregorio : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 4f;          // Velocidad de movimiento en plano
    public float gravedad = -9.81f;       // Fuerza de gravedad

    private CharacterController controller;
    private Vector3 velocity;             // Vector para la gravedad

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // --- Movimiento horizontal controlado por input ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Solo mover si hay input
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * velocidad * Time.deltaTime);

        // --- Gravedad constante ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // pequeño empuje hacia abajo para mantenerlo pegado
        }

        velocity.y += gravedad * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}