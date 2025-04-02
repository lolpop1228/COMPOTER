using System.Collections;
using UnityEngine;
using UnityEngine.UI; // For Unity UI Text
using TMPro; // For TextMeshPro support

public class BigAttack : MonoBehaviour
{
    public GameObject bigAttackPrefabs;
    public Transform attackPoint;
    public GameObject platforms;
    public GameObject warningUI; // Reference to warning UI (Text or TextMeshPro)

    private Text uiText; // For Unity UI Text
    private TMP_Text tmpText; // For TextMeshPro

    void Start()
    {
        // Get Text components (if available)
        if (warningUI != null)
        {
            uiText = warningUI.GetComponent<Text>();
            tmpText = warningUI.GetComponent<TMP_Text>();
        }
    }

    void OnEnable()
    {
        Instantiate(bigAttackPrefabs, attackPoint.position, attackPoint.rotation);
        platforms.SetActive(true);

        StartCoroutine(PlatformWarning());
    }

    IEnumerator PlatformWarning()
    {
        warningUI?.SetActive(true);

        yield return new WaitForSeconds(10f);

        UpdateWarningText("Get off the platforms!!");

        yield return new WaitForSeconds(5f);

        warningUI?.SetActive(false);
        platforms.SetActive(false);
    }

    void UpdateWarningText(string message)
    {
        if (uiText != null)
            uiText.text = message;
        if (tmpText != null)
            tmpText.text = message;
    }
}
