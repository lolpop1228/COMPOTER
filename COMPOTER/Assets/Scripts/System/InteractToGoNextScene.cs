using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractToGoNextScene : MonoBehaviour, IInteractable
{
    public string sceneToGo;

    public void Interact()
    {
        SceneManager.LoadScene(sceneToGo);
    }
}
