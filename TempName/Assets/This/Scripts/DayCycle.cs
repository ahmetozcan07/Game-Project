using UnityEngine;

public class DayCycle : MonoBehaviour
{
    private const float CycleDuration = 360f; //6 dakika
    public float currentRotation;
    private float rotationSpeed; 

    private void Start()
    {
        CalculateRotationSpeed();
        UpdateLightRotation();
    }

    private void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;

        if (currentRotation >= 360f)
        {
            currentRotation -= 360f;
        }

        UpdateLightRotation();
    }

    private void CalculateRotationSpeed()
    {
        rotationSpeed = 360f / CycleDuration;
    }

    private void UpdateLightRotation()
    {
        transform.GetChild(0).rotation = Quaternion.Euler(currentRotation, 0f, 0f);
    }
}
