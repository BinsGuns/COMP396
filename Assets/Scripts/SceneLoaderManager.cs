using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{

    public static SceneLoaderManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void LoadHomeScreen()
    {
        // Load the home screen
        SceneManager.LoadScene(0);
    }
    public void LoadTutorialWorld()
    {
        // Load the game world
        SceneManager.LoadScene(1);
    }
    public void LoadGameWorld()
    {
        // Load the game world
        SceneManager.LoadScene(2);
    }

    public void LoadMultiplayerGame()
    {
        // multiplayer
        SceneManager.LoadScene(3);
    }
}
