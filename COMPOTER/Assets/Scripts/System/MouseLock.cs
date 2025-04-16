using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseLock : MonoBehaviour
{
    public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(GoMenu());
    }

    IEnumerator GoMenu()
    {
        yield return new WaitForSeconds(105);
        LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
