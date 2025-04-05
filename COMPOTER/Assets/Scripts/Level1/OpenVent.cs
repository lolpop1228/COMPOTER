using UnityEngine;
using UnityEngine.UI;

public class OpenVent : MonoBehaviour
{
    public Camera playerCamera; 
    public GameObject uiPanel;
    public float maxDistance = 3f;
    public string ventTag = "Vent";

       private void Update()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.CompareTag(ventTag))
            {
                if (!uiPanel.activeSelf)
                {
                    uiPanel.SetActive(true);
                }
            }
            else
            {
                if (uiPanel.activeSelf)
                {
                    uiPanel.SetActive(false);
                }
            }
        }
        else
        {
            if (uiPanel.activeSelf)
            {
                uiPanel.SetActive(false);
            }
        }
    }
}
