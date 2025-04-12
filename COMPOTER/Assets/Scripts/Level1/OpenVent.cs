using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class OpenVent : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject uiPanel;
    public float maxDistance = 3f;
    public string[] objectTags = { "RightVent", "LeftVent" };
    private bool isUIPanelActive = false;

    private bool hasInteractedWithLeftVent = false;
    private bool hasInteractedWithRightVent = false;

    public PlayableDirector rightVentDirector;
    public PlayableDirector leftVentDirector;

    public AudioClip rightVentSound;
    public AudioClip leftVentSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (hasInteractedWithLeftVent && hasInteractedWithRightVent)
        {
            if (isUIPanelActive)
            {
                uiPanel.SetActive(false);
                isUIPanelActive = false;
            }
            return;
        }

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            bool isTagMatched = false;

            foreach (string tag in objectTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    isTagMatched = true;
                    break;
                }
            }

            if (isTagMatched && !isUIPanelActive)
            {
                if (hit.collider.CompareTag("RightVent") && !hasInteractedWithRightVent || 
                    hit.collider.CompareTag("LeftVent") && !hasInteractedWithLeftVent)
                {
                    uiPanel.SetActive(true);
                    isUIPanelActive = true;
                }
            }
            else if (!isTagMatched && isUIPanelActive)
            {
                uiPanel.SetActive(false);
                isUIPanelActive = false;
            }

            if (hit.collider.CompareTag("RightVent") && Input.GetKeyDown(KeyCode.E) && !hasInteractedWithRightVent)
            {
                rightVentDirector.Play();
                PlaySound(rightVentSound);
                hasInteractedWithRightVent = true;
                uiPanel.SetActive(false);
                isUIPanelActive = false;
            }

            if (hit.collider.CompareTag("LeftVent") && Input.GetKeyDown(KeyCode.E) && !hasInteractedWithLeftVent)
            {
                leftVentDirector.Play();
                PlaySound(leftVentSound);
                hasInteractedWithLeftVent = true;
                uiPanel.SetActive(false);
                isUIPanelActive = false;
            }
        }
        else
        {
            if (isUIPanelActive)
            {
                uiPanel.SetActive(false);
                isUIPanelActive = false;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
