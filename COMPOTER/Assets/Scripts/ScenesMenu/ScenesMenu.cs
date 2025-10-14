using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesMenu : MonoBehaviour
{
    public string sceneToLoad;
    public string sceneToLoad2;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FightPensagon()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void FightKemali()
    {
        SceneManager.LoadScene(sceneToLoad2);
    }
}
