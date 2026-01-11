using UnityEngine;

public class PanoramicSkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 0.2f; // velocidad lenta y ritual

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
