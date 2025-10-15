using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BetterBossMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("ScenesMenu");
    }
}
