using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotationSpeed = 0.5f; // Adjust this value in the Inspector

    void Update()
    {
        // Access the global RenderSettings and the current skybox material
        // Set the "_Rotation" property based on time and rotationSpeed
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}