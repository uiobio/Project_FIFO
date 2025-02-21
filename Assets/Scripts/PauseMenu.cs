using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if  (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else 
                PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // making pause menu visible
        Time.timeScale = 0f; // freezing game time
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // hiding pause menu
        Time.timeScale = 1f; // resuming game time
        isPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // resetting time before quitting
        Application.Quit(); // completely closing the game 
    }
}
