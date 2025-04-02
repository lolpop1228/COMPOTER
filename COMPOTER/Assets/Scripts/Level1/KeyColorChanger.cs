using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyPressColorChange : MonoBehaviour
{
    // Create a dictionary to map KeyCode to Image
    private Dictionary<KeyCode, Image> keyImageMap = new Dictionary<KeyCode, Image>();

    // Default and pressed colors
    private Color defaultColor = Color.white;
    private Color pressedColor = Color.gray;

    void Start()
    {
        // Initialize the key-image mapping
        keyImageMap[KeyCode.W] = wKeyImage;
        keyImageMap[KeyCode.A] = aKeyImage;
        keyImageMap[KeyCode.S] = sKeyImage;
        keyImageMap[KeyCode.D] = dKeyImage;
        keyImageMap[KeyCode.Space] = spacebarImage;
        keyImageMap[KeyCode.LeftShift] = leftShiftImage;
        keyImageMap[KeyCode.LeftControl] = leftCtrlImage;
        keyImageMap[KeyCode.E] = eKeyImage;
        keyImageMap[KeyCode.F] = fKeyImage;
    }

    void Update()
    {
        // Loop through each key in the dictionary
        foreach (var key in keyImageMap.Keys)
        {
            // Check if the key is pressed
            if (Input.GetKeyDown(key))
            {
                keyImageMap[key].color = pressedColor;
            }
            // Check if the key is released
            else if (Input.GetKeyUp(key))
            {
                keyImageMap[key].color = defaultColor;
            }
        }
    }

    // References to the UI Image components in the inspector
    public Image wKeyImage;
    public Image aKeyImage;
    public Image sKeyImage;
    public Image dKeyImage;
    public Image spacebarImage;
    public Image leftShiftImage;
    public Image leftCtrlImage;
    public Image eKeyImage;
    public Image fKeyImage;
}
