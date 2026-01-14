using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public float sensibilidad = 2f;
    float rotacionX = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);

        // Rotación vertical (cámara)
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        // Rotación horizontal (cuerpo del personaje)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
