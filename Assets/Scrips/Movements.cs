using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movements : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float jumpHeight = 2.0f;

    [Header("Gravity Settings")]
    public float gravity = -9.81f * 3; 
    private const float GroundedPush = -2.0f;

    private CharacterController characterController;
    private InputManager inputManager;
    private Animator animator; 

    private Vector3 moveDirection;
    private float jumpVelocity;

    // Nueva variable: referencia a la cámara
    private Transform cameraTransform;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>(); 
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform; // referencia directa a la cámara principal

        if (animator == null)
            Debug.LogWarning("Animator no encontrado. La animación de caminar no funcionará.");
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // --- SALTO Y GRAVEDAD ---
        if (characterController.isGrounded)
        {
            jumpVelocity = GroundedPush;

            if (inputManager.jumpTriggered)
            {
                jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                inputManager.jumpTriggered = false;
            }
        }
        else
        {
            jumpVelocity += gravity * Time.deltaTime;
        }

        // --- MOVIMIENTO BASADO EN LA CÁMARA ---
        Vector2 input = inputManager.horizontalInput;

        // Obtener direcciones según la cámara (no el jugador)
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ignorar inclinación vertical de la cámara (solo movimiento horizontal)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Combinar WASD con la orientación de la cámara
        Vector3 desiredMove = (forward * input.y + right * input.x).normalized;

        moveDirection.x = desiredMove.x * moveSpeed;
        moveDirection.z = desiredMove.z * moveSpeed;
        moveDirection.y = jumpVelocity;

        // Aplicar movimiento
        characterController.Move(moveDirection * Time.deltaTime);

        // --- ANIMACIÓN (opcional) ---
        if (animator != null)
        {
            float currentSpeed = new Vector3(moveDirection.x, 0, moveDirection.z).magnitude;
            float smoothSpeed = Mathf.Lerp(animator.GetFloat("Speed"), currentSpeed, Time.deltaTime * 10f);
            animator.SetFloat("Speed", smoothSpeed);
        }
    }
}
