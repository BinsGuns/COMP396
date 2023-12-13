using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject playerhomescreenUI;
    public GameObject newgameUI;
    public GameObject ContainerMultiplayer;
    public FirebaseManager FirebaseManager;
    public GameObject MultiplayerTextField;
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

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }
    public void RegisterScreen() // Regester button
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }

    public void PlayerHomeScreen() //Back button
    {
        loginUI.SetActive(false);
        playerhomescreenUI.SetActive(true);
    }

    public void NewGameScreen() //Back button
    {
        playerhomescreenUI.SetActive(false);
        newgameUI.SetActive(true);
        ContainerMultiplayer.SetActive(false);
        FirebaseManager._isMultiplayer = false;
    }

    public void MultiplayerGame() // multiplayer button
    {
        ContainerMultiplayer.SetActive(true);
        //playerhomescreenUI.SetActive(false);
       // newgameUI.SetActive(true);
        FirebaseManager._isMultiplayer = true;
    }

    public void GameModeHost()
    {
        
        playerhomescreenUI.SetActive(false);
        newgameUI.SetActive(true);
        FirebaseManager._isHost = true;
        MultiplayerTextField.SetActive(true);
    }
    
    public void GameModeJoin()
    {
       
        playerhomescreenUI.SetActive(false);
        newgameUI.SetActive(true);
        FirebaseManager._isHost = false;
        MultiplayerTextField.SetActive(true);
    }
    
    
}
