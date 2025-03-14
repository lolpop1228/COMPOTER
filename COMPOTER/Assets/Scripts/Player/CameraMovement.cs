using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    public Slider sensXSlider;
    public Slider sensYSlider;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sensX = PlayerPrefs.GetFloat("SensX", 100f);
        sensY = PlayerPrefs.GetFloat("SensY", 100f);

        if (sensXSlider && sensYSlider)
        {
            sensXSlider.value = sensX;
            sensYSlider.value = sensY;

            sensXSlider.onValueChanged.AddListener(UpdateSensX);
            sensYSlider.onValueChanged.AddListener(UpdateSensY);
        }
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public void UpdateSensX(float newSensX)
    {
        sensX = newSensX;
        PlayerPrefs.SetFloat("SensX", sensX); // Save the new sensitivity
    }

    public void UpdateSensY(float newSensY)
    {
        sensY = newSensY;
        PlayerPrefs.SetFloat("SensY", sensY); // Save the new sensitivity
    }
}
