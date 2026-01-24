using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject thirdPersonCamera;   // Cámara de tercera persona
    public GameObject firstPersonCamera;   // Cámara de primera persona

    [Header("Pivotes y referencias")]
    public Transform playerRoot;           // Raíz del jugador (Gregorio)
    public Transform shoulderPivot;        // Pivote vertical (pitch)
    public Transform cameraHolder;         // Objeto que contiene la Main Camera

    [Header("Rotación")]
    public float rotationSpeed = 300f;
    public float minPitch = -70f;
    public float maxPitch = 70f;

    [Header("Suavizado")]
    public float positionSmoothTime = 0.15f;
    public float rotationLerp = 6f;

    private float yaw;
    private float pitch;
    private Vector3 currentVelocity;

    void Start()
    {
        // Inicia en tercera persona
        thirdPersonCamera.SetActive(true);
        firstPersonCamera.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Alternar cámaras con tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isThirdActive = thirdPersonCamera.activeSelf;
            thirdPersonCamera.SetActive(!isThirdActive);
            firstPersonCamera.SetActive(isThirdActive);
        }

        // Entrada del mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotación vertical (pitch)
        if (shoulderPivot != null)
        {
            shoulderPivot.localRotation = Quaternion.Lerp(
                shoulderPivot.localRotation,
                Quaternion.Euler(pitch, 0, 0),
                Time.deltaTime * rotationLerp
            );
        }

        // Rotación horizontal (yaw)
        if (playerRoot != null)
        {
            playerRoot.rotation = Quaternion.Lerp(
                playerRoot.rotation,
                Quaternion.Euler(0, yaw, 0),
                Time.deltaTime * rotationLerp
            );
        }

        // Suavizado de la cámara principal (Main Camera)
        if (cameraHolder != null)
        {
            Transform activeCam = thirdPersonCamera.activeSelf ? thirdPersonCamera.transform : firstPersonCamera.transform;

            cameraHolder.position = Vector3.SmoothDamp(
                cameraHolder.position,
                activeCam.position,
                ref currentVelocity,
                positionSmoothTime
            );

            cameraHolder.rotation = Quaternion.Slerp(
                cameraHolder.rotation,
                activeCam.rotation,
                Time.deltaTime * rotationLerp
            );
        }
    }
}