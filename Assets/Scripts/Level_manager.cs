using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_manager : MonoBehaviour
{
    public static Level_manager instance;
    private bool isPaused = false;

    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button quitButton;

    private void Awake() //Makes levelmanager callable in any script: Level_manager.instance.[]
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(TogglePause);
        quitButton.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){ // Detects when 'P' is pressed
            TogglePause();
        }
    }

    void TogglePause(){
        isPaused = !isPaused;

        if (isPaused) {
            pauseMenuUI.SetActive(true); // show menu
            Time.timeScale = 0f; // pause game
        }
        else {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f; // resume game
        }
    }

    void QuitGame()
    {
        Application.Quit(); // quitting game
    }
}
