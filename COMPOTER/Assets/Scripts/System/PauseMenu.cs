using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainPauseMenu;
    public GameObject settingsMenu;
    public MonoBehaviour[] scriptToDisable;

    public bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        //Disable all script
        foreach (MonoBehaviour script in scriptToDisable)
        {
            script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        foreach (MonoBehaviour script in scriptToDisable)
        {
            script.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Settings()
    {
        mainPauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void Return()
    {
        mainPauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
