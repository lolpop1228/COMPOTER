using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject settings;
    public GameObject menu;
    public GameObject controls;
    public GameObject credits;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        settings.SetActive(false);
        controls.SetActive(false);
        credits.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Settings()
    {
        settings.SetActive(true);
        controls.SetActive(false);
        credits.SetActive(false);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }

    public void Controls()
    {
        controls.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);
    }

    public void CloseControls()
    {
        controls.SetActive(false);
    }

    public void Credits()
    {
        credits.SetActive(true);
        controls.SetActive(false);
        settings.SetActive(false);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Quit!!");
        Application.Quit();
    }

}
