using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Asegúrate de que este nombre de clase coincida con el generado por tu Input Actions.
    @PlayersControls controls;
    @PlayersControls.GroundMovementsActions groundMovement;
    
    // --- Variables de ENTRADA (accesibles por otros scripts) ---
    [HideInInspector] public Vector2 horizontalInput; // WASD (Move)
    [HideInInspector] public Vector2 lookInput;       // Mouse X/Y (Look)
    [HideInInspector] public bool jumpTriggered;      // Salto

    private void Awake()
    {
        controls = new @PlayersControls();
        groundMovement = controls.GroundMovements; 
        
        // 1. WASD (Move)
        groundMovement.HorizontalMovement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
        groundMovement.HorizontalMovement.canceled += ctx => horizontalInput = Vector2.zero;

        // 2. Salto (Jump)
        groundMovement.Jump.performed += ctx => jumpTriggered = true; 
        
        // 3. Look Input (Mouse X y Mouse Y): Captura la entrada del ratón.
        groundMovement.MouseX.performed += ctx => lookInput.x = ctx.ReadValue<float>();
        groundMovement.MouseX.canceled += ctx => lookInput.x = 0f;
        
        groundMovement.MouseY.performed += ctx => lookInput.y = ctx.ReadValue<float>();
        groundMovement.MouseY.canceled += ctx => lookInput.y = 0f;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}