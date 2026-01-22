using UnityEngine;

public class SkyDomeRotator : MonoBehaviour
{
    public float rotationSpeed = 2f; // velocidad de giro

    void Update()
    {
        // Gira la esfera alrededor del eje Y (vertical)
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}
