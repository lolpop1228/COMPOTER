using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class DoorPIN : MonoBehaviour, IInteractable
{
    public GameObject uiPIN;
    public float activationRange = 3f;
    [SerializeField] private TMP_Text Ans;
    private string Answer = "254";
    public PlayerMovement playerMovement;
    public CameraMovement cameraMovement;
    private bool isInteracting = false;
    private bool isUnlocked = false;

    public PlayableDirector playableDirector;

    public AudioClip successSound;
    public AudioClip invalidSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInteracting && Input.GetKeyDown(KeyCode.Backspace))
        {
            UnlockPlayerControls();
        }
    }

    public void Interact()
    {
        if (!isUnlocked)
        {
            uiPIN.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            LockPlayerControls(true);
            isInteracting = true;
        }
    }

    public void Number(int number)
    {
        if (Ans.text.Length < 3)
        {
            Ans.text += number.ToString();
            PlaySound(clickSound);
        }
    }

    public void Enter()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "CORRECT";
            PlayTimeline();
            PlaySound(successSound);
            UnlockPlayerControls();
            isUnlocked = true;
        }
        else
        {
            Ans.text = "INVALID";
            PlaySound(invalidSound);
            Invoke("ClearInput", 1f);
        }
    }

    private void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void ClearInput()
    {
        Ans.text = "";
    }

    private void LockPlayerControls(bool lockControls)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = !lockControls;
        }

        if (cameraMovement != null)
        {
            cameraMovement.enabled = !lockControls;
        }
    }

    public void UnlockPlayerControls()
    {
        LockPlayerControls(false);
        isInteracting = false;

        uiPIN.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
