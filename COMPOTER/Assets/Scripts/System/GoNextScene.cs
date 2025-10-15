using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNextScene : MonoBehaviour
{
    public string sceneToGo;
    public CharacterController playerController;

    void TeleportPlayer()
    {
        playerController.enabled = false;

        SceneManager.LoadScene(sceneToGo);

        playerController.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerController != null)
            {
                TeleportPlayer();
            }
        }
    }
}
