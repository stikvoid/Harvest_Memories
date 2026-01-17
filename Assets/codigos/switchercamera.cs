using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject thirdPersonCamera;
    public GameObject firstPersonCamera;

    [Header("Pivotes y referencias")]
    public Transform playerRoot;      // El transform del jugador (para yaw)
    public Transform shoulderPivot;   // Eje vertical para tercera persona
    public Transform cameraHolder;    // Posición/rotación objetivo de la cámara activa

    [Header("Rotación")]
    public float rotationSpeed = 280f;
    public float minPitch = -70f;
    public float maxPitch = 70f;

    [Header("Suavizado")]
    public float positionSmoothTime = 0.08f; // SmoothDamp (posición)
    public float rotationLerp = 8f;          // Slerp (rotación)
    private Vector3 posVelocity = Vector3.zero;

    // Estado interno
    private Transform cam;           // Transform de la cámara activa
    private float yaw;               // Rotación horizontal acumulada (en grados)
    private float pitch;             // Rotación vertical acumulada (en grados)

    void Start()
    {
        // Validaciones mínimas
        if (playerRoot == null) playerRoot = transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Activar tercera persona por defecto
        SetThirdPerson(true);
    }

    void Update()
    {
        // Alternar cámaras con C
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool toThird = !thirdPersonCamera.activeSelf;
            SetThirdPerson(toThird);
        }

        HandleRotation();
        HandleCameraFollow();
    }

    void SetThirdPerson(bool enableThird)
    {
        thirdPersonCamera.SetActive(enableThird);
        firstPersonCamera.SetActive(!enableThird);

        cam = enableThird ? thirdPersonCamera.transform : firstPersonCamera.transform;

        // Recalcular yaw/pitch desde el estado actual para evitar saltos
        yaw = playerRoot.eulerAngles.y;

        if (enableThird && shoulderPivot != null)
        {
            // Tomar pitch desde el pivot
            pitch = shoulderPivot.localEulerAngles.x;
            // Normalizar a rango [-180, 180]
            if (pitch > 180f) pitch -= 360f;
        }
        else
        {
            // En primera persona, tomar pitch desde la cámara
            pitch = cam.localEulerAngles.x;
            if (pitch > 180f) pitch -= 360f;
        }
    }

    void HandleRotation()
    {
        // Solo rotar si el click izquierdo está presionado
        if (!Input.GetMouseButton(0)) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Acumular yaw/pitch
        yaw += mouseX;
        pitch = Mathf.Clamp(pitch - mouseY, minPitch, maxPitch);

        // Aplicar yaw suavizado al jugador (Slerp hacia el objetivo)
        Quaternion targetYaw = Quaternion.Euler(0f, yaw, 0f);
        playerRoot.rotation = Quaternion.Slerp(playerRoot.rotation, targetYaw, rotationLerp * Time.deltaTime);

        // Aplicar pitch según cámara activa
        if (thirdPersonCamera.activeSelf && shoulderPivot != null)
        {
            // Pitch directo en el pivot (sin Lerp para evitar “rebotes”)
            shoulderPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        else if (firstPersonCamera.activeSelf && cam != null)
        {
            // Pitch directo en la cámara FP
            cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    void HandleCameraFollow()
    {
        if (cam == null || cameraHolder == null) return;

        // Posición con SmoothDamp (suave, sin vibración)
        cam.position = Vector3.SmoothDamp(cam.position, cameraHolder.position, ref posVelocity, positionSmoothTime);

        // Rotación con Slerp (fluida, sin “pelea” con la rotación de yaw/pitch)
        cam.rotation = Quaternion.Slerp(cam.rotation, cameraHolder.rotation, rotationLerp * Time.deltaTime);
    }
}