using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public GameObject cube;             // The cube you want to move
    public Vector3 originalPosition;    // Store the original position of the cube
    public float interactionRange = 3f; // Range at which the player can interact with the button
    
    private Transform player;           // The player's transform
    
    void Start()
    {
        // Store the original position of the cube at the start of the game
        originalPosition = cube.transform.position;

        // Get the player's transform (assuming you have a player with a tag "Player")
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Check if the player is close to the button and presses the 'E' key
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ResetCubePosition();
            }
        }
    }

    // Reset the cube's position to the original position
    void ResetCubePosition()
    {
        cube.transform.position = originalPosition;
    }
}
