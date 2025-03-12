using UnityEngine;

public class RecoilSystem : MonoBehaviour
{
    private Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
    public Transform cam; // Ensure this is assigned if needed

    [SerializeField] float recoilX = 2f;
    [SerializeField] float recoilY = 2f;
    [SerializeField] float recoilZ = 2f;

    [SerializeField] float kickBackZ = 0.1f;

    public float snappiness = 6f;
    public float returnAmount = 5f;

    private void Start()
    {
        initialGunPosition = transform.localPosition;
        currentPosition = initialGunPosition; // Initialize correctly
    }

    private void Update()
    {
        // Smoothly return rotation to zero
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRotation);

        // Apply position recoil recovery
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = currentPosition;
    }

    public void Recoil()
    {
        Debug.Log("Recoil Triggered!"); // Debug log to check if it's working

        targetPosition -= new Vector3(0, 0, kickBackZ); // Apply kickback
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
