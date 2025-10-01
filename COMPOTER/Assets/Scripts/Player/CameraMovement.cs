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

    // Reference to recoil script
    private CameraRecoil cameraRecoil;

    private void Start()
    {
        HideMouseCursor();

        sensX = PlayerPrefs.GetFloat("SensX", 100f);
        sensY = PlayerPrefs.GetFloat("SensY", 100f);

        if (sensXSlider && sensYSlider)
        {
            sensXSlider.value = sensX;
            sensYSlider.value = sensY;

            sensXSlider.onValueChanged.AddListener(UpdateSensX);
            sensYSlider.onValueChanged.AddListener(UpdateSensY);
        }

        // Get the CameraRecoil component if it exists
        cameraRecoil = GetComponent<CameraRecoil>();
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Get recoil offset if the component exists
        Vector3 recoilOffset = Vector3.zero;
        if (cameraRecoil != null)
        {
            recoilOffset = cameraRecoil.GetRecoilOffset();
        }

        // Apply mouse look + recoil offset
        transform.localRotation = Quaternion.Euler(
            xRotation + recoilOffset.x, 
            yRotation + recoilOffset.y, 
            recoilOffset.z
        );
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void UpdateSensX(float newSensX)
    {
        sensX = newSensX;
        PlayerPrefs.SetFloat("SensX", sensX);
    }

    public void UpdateSensY(float newSensY)
    {
        sensY = newSensY;
        PlayerPrefs.SetFloat("SensY", sensY);
    }

    public void HideMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowMouseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}