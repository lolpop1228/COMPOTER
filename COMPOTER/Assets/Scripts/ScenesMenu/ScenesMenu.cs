using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesMenu : MonoBehaviour
{
    public string sceneLevel1;
    public string sceneLevel2;
    public string sceneGunfights;
    public string scenePensagonEasy;
    public string scenePensagonMed;
    public string scenePensagonHard;
    public string sceneKemaliEasy;
    public string sceneKemaliMed;
    public string sceneKemaliHard;
    public GameObject PuzzlesPanel;
    public GameObject BattlesPanel;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PuzzlesPanel.SetActive(false);
        BattlesPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Puzzles()
    {
        PuzzlesPanel.SetActive(true);
        BattlesPanel.SetActive(false);
    }

    public void ClosePuzzles()
    {
        PuzzlesPanel.SetActive(false);
    }

    public void Battles()
    {
        BattlesPanel.SetActive(true);
        PuzzlesPanel.SetActive(false);
    }

    public void CloseBattles()
    {
        BattlesPanel.SetActive(false);
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene(sceneLevel1);
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene(sceneLevel2);
    }

    public void PlayGunfights()
    {
        SceneManager.LoadScene(sceneGunfights);
    }

    public void PlayPensagonEasy()
    {
        SceneManager.LoadScene(scenePensagonEasy);
    }

    public void PlayPensagonMed()
    {
        SceneManager.LoadScene(scenePensagonMed);
    }

    public void PlayPensagonHard()
    {
        SceneManager.LoadScene(scenePensagonHard);
    }

    public void PlayKemaliEasy()
    {
        SceneManager.LoadScene(sceneKemaliEasy);
    }

    public void PlayKemaliMed()
    {
        SceneManager.LoadScene(sceneKemaliMed);
    }

    public void PlayKemaliHard()
    {
        SceneManager.LoadScene(sceneKemaliHard);
    }
}
