using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public bool isQuitDoor = false; // Flag to determine if this is the quit door

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isQuitDoor)
            {
                // Quit the application
                Debug.Log("Quitting the game...");
                Application.Quit();

                // Note: Application.Quit will not work in the editor; use UnityEditor.EditorApplication.isPlaying = false; for testing in the editor.
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
            else
            {
                // Handle other door functionality here
                Debug.Log("This is another room door.");
            }
        }
    }
}
