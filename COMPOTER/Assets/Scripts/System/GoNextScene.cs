using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNextScene : MonoBehaviour, IInteractable
{
    public string sceneToGo;

    public void Interact()
    {
        SceneManager.LoadScene(sceneToGo);
    }

}
