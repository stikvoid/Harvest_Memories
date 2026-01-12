using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class logicaJugadorPrincipal : MonoBehaviour
{
    // ───────── MOVIMIENTO ─────────
    public float velocidadMovimiento = 5f;
    private float x, y;
    private Vector3 movimiento;

    // ───────── ANIMACIÓN ─────────
    public Animator anim;

    // ───────── CÁMARA ─────────
    public Transform cameraShoulder;
    public Transform cameraHolder;
    private Transform cam;
    private float rotY;
    public float rotationSpeed = 120f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSmooth = 10f;

    // ───────── SALTO ─────────
    public float jumpForce = 7.5f;
    public bool OnGround;
    public float groundCheckDistance = 0.4f;

    // ───────── VELOCIDADES ─────────
    private float velocidadInicial;
    private float velocidadAgachado;
    private float velocidadCorrer;
    private bool isCrouching;

    // ───────── ESTADOS ─────────
    private bool isPicking = false;
    private bool velaActiva = false;

    // ───────── COMPONENTES ─────────
    private Rigidbody rb;
    private Transform tr;

    void Start()
    {
        velocidadInicial = velocidadMovimiento;
        velocidadAgachado = velocidadMovimiento * 0.5f;
        velocidadCorrer = velocidadMovimiento * 2f;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        tr = transform;
        cam = Camera.main.transform;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        Physics.gravity = new Vector3(0, -30f, 0);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckGround();

        if (!isPicking)
        {
            LeerMovimiento();
            ControlCamara();
            ControlAcciones();
            ControlVela();
        }

        ControlRecoger();

        // ───── ANIMACIONES BASE ─────
        anim.SetFloat("velX", x);
        anim.SetFloat("velY", y);
        anim.SetBool("ground", OnGround);
    }

    void FixedUpdate()
    {
        if (isPicking) return;

        rb.MovePosition(rb.position + movimiento);
    }

    // ───────── MOVIMIENTO ─────────
    void LeerMovimiento()
    {
        if (Keyboard.current == null) return;

        x = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        y = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);

        Vector3 direccion = new Vector3(x, 0, y).normalized;
        movimiento = tr.TransformDirection(direccion) * velocidadMovimiento * Time.fixedDeltaTime;
    }

    // ───────── ACCIONES ─────────
    void ControlAcciones()
    {
        if (Keyboard.current == null) return;

        // SALTO
        if (Keyboard.current.spaceKey.wasPressedThisFrame && OnGround)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            OnGround = false;
        }

        // AGACHARSE
        if (Keyboard.current.leftCtrlKey.isPressed)
        {
            anim.SetBool("agachado", true);
            velocidadMovimiento = velocidadAgachado;
            isCrouching = true;
        }
        else
        {
            anim.SetBool("agachado", false);
            velocidadMovimiento = velocidadInicial;
            isCrouching = false;
        }

        // CORRER
        if (Keyboard.current.leftShiftKey.isPressed && !isCrouching)
        {
            anim.SetBool("correr", true);
            velocidadMovimiento = velocidadCorrer;
        }
        else
        {
            anim.SetBool("correr", false);
            if (!isCrouching)
                velocidadMovimiento = velocidadInicial;
        }
    }

    // ───────── VELA ─────────
    void ControlVela()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            velaActiva = !velaActiva;
            anim.SetTrigger("vela");
        }
    }

    // ───────── RECOGER ─────────
    void ControlRecoger()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame && !isPicking)
        {
            StartCoroutine(Recoger());
        }
    }

    IEnumerator Recoger()
    {
        isPicking = true;

        movimiento = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        anim.SetBool("Recoger", true);

        yield return new WaitForSeconds(1.2f);

        anim.SetBool("Recoger", false);
        isPicking = false;
    }

    // ───────── CÁMARA ─────────
    void ControlCamara()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * rotationSpeed * Time.deltaTime;
        float mouseY = mouseDelta.y * rotationSpeed * Time.deltaTime;

        tr.Rotate(Vector3.up * mouseX);

        rotY -= mouseY;
        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);
        cameraShoulder.localRotation = Quaternion.Euler(rotY, 0, 0);

        cam.position = Vector3.Lerp(cam.position, cameraHolder.position, cameraSmooth * Time.deltaTime);
        cam.rotation = Quaternion.Lerp(cam.rotation, cameraHolder.rotation, cameraSmooth * Time.deltaTime);
    }

    // ───────── SUELO ─────────
    void CheckGround()
    {
        Vector3 origen = transform.position + Vector3.up * 0.15f;
        OnGround = Physics.Raycast(origen, Vector3.down, groundCheckDistance);
        Debug.DrawRay(origen, Vector3.down * groundCheckDistance, Color.green);
    }
}
