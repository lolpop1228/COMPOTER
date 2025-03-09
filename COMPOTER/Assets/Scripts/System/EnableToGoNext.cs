using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnableToGoNext : MonoBehaviour
{
    public string sceneToLoad;

    private void OnEnable()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
