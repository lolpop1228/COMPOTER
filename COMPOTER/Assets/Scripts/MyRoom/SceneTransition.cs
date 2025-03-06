using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour, IInteractable
{
    public string sceneToLoad;
    public Image fadeImage;
    public float fadeDuration = 1f;

    public void Interact()
    {
        StartCoroutine(LoadSceneWithFade());
    }

    private IEnumerator LoadSceneWithFade()
    {
        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer< fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = new Color(0,0,0, timer / fadeDuration);
            yield return null;
        }
    }
}
