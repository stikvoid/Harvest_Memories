using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject thirdPersonCamera;   // Arrastra aquí tu cámara de tercera persona
    public GameObject firstPersonCamera;   // Arrastra aquí tu cámara de primera persona

    void Start()
    {
        // Al inicio, activa solo la cámara de tercera persona
        thirdPersonCamera.SetActive(true);
        firstPersonCamera.SetActive(false);
    }

    void Update()
    {
        // Alternar con la tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            bool isThirdActive = thirdPersonCamera.activeSelf;

            thirdPersonCamera.SetActive(!isThirdActive);
            firstPersonCamera.SetActive(isThirdActive);
        }
    }
}