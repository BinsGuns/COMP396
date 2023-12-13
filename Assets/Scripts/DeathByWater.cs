using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathByWater : MonoBehaviour
{
    public GameObject gameOverPanel;

    private bool isGameOver = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the triggering object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered water trigger");
            // Player triggered the water, set game over panel to active
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                Destroy(other.gameObject);
            }

            // Set the game over flag
            isGameOver = true;

            // Wait for 5 seconds and then restart the scene
            Invoke("RestartScene", 5f);
        }
        else
        {
            
        }
    }

    private void RestartScene()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
