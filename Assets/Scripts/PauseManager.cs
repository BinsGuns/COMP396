using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button saveButton;
    public Button optionsButton;
    public GameObject pauseButtonObject;

    private bool isPaused = false;

    void Start()
    {
        
    }

    private void Update()
    {

        // Check for user input to toggle the pause state
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }


        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseGame);
        }
        else
        {
            Debug.LogWarning("Pause Button not assigned to the script.");
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        else
        {
            Debug.LogWarning("Resume Button not assigned to the script.");
        }
    }


    void TogglePause()
    {
        // Toggle the pause state
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0f;
        isPaused = true;

        // Activate the pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            saveButton.interactable = false;
            optionsButton.interactable = false;
            pauseButtonObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Pause Panel not assigned to the script.");
        }
    }

    void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1f;
        isPaused = false;

        // Deactivate the pause panel
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            saveButton.interactable = true;
            optionsButton.interactable = true;
            pauseButtonObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Pause Panel not assigned to the script.");
        }
    }
}
