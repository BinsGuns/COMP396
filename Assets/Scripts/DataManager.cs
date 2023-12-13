using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{

    // Singleton instance
    public static DataManager instance;

    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("User Data")]
    public TMP_Text playerNameText;
    public Player player;
    HealthBar healthBar;
    ManaBar manaBar;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        healthBar = GameObject.FindObjectOfType<HealthBar>();
        manaBar = GameObject.FindObjectOfType<ManaBar>();

        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when the script is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);
        /**
        // Load data only if the loaded scene is not TutorialScene
        if (scene.name != "TutorialScene")
        {
            LoadDataBetweenScenes();
            Debug.Log("Data loaded between scenes");
        }
        **/
    }

    // Start is called before the first frame update
    void Start()
    {

        GetPlayerData();
    }

    void Update()
    {
        // Check if the resume flag is true
        if (PlayerPrefs.GetInt("ResumeFlagKey", 0) == 1)
        {
            // Call the method to get player data
            GetPlayerData();

            // Reset the resume flag after getting player data
            PlayerPrefs.SetInt("ResumeFlagKey", 0);
        }

    
    }


    [System.Serializable]
    public class IntListWrapper
    {
        public List<int> stackList;
    }


    [System.Serializable]
    public class Vector3Wrapper
    {
        public float x;
        public float y;
        public float z;

        public Vector3Wrapper(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }



    public void GetPlayerData()
    {
      
          // Load data for the first time
          LoadDataForFirstTime();
          // Set the flag to indicate that it's not the first time anymore
      
    }

    public void LoadDataForFirstTime()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();

            if (player != null)
            {
                player.hitPoints = int.Parse(PlayerPrefs.GetString("HitPoints"));
                healthBar.SetMaxHealthForHealthBar(player.maxHitPoints);
                healthBar.SetHealth(player.hitPoints);

                player.manaPoints = int.Parse(PlayerPrefs.GetString("ManaPoints"));
                manaBar.SetMaxManaForManaBar(player.maxMana);
                manaBar.SetMana(player.manaPoints);
            }
            else
            {
                Debug.LogError("Player component not found on the playerObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found with the tag 'Player'.");
        }



        /**
        player.hitPoints = int.Parse(PlayerPrefs.GetString("HitPoints"));
        healthBar.SetMaxHealthForHealthBar(player.maxHitPoints);
        healthBar.SetHealth(player.hitPoints);

        player.manaPoints = int.Parse(PlayerPrefs.GetString("ManaPoints"));
        manaBar.SetMaxManaForManaBar(player.maxMana);
        manaBar.SetMana(player.manaPoints);
        **/


        // Deserialize the JSON string to Vector3
        string jsonPosition = PlayerPrefs.GetString("PlayerPosition"); ; // Your JSON string here
        Vector3Wrapper positionWrapper = JsonUtility.FromJson<Vector3Wrapper>(jsonPosition);
        Vector3 playerPosition = positionWrapper.ToVector3();
        player.transform.position = playerPosition;



        string jsonEquipment = PlayerPrefs.GetString("Equipment");
        List<Items> deserializedEquipmentList = JsonHelper.FromJson<Items>(jsonEquipment);
        EquipmentManager.instance.equipmentList = deserializedEquipmentList;
        EquipmentManager.instance.RefreshEquipment();



        //**********************FOR INVENTORY***********************************
        string json = PlayerPrefs.GetString("Inventory");
        List<Items> deserializedItemsList = JsonHelper.FromJson<Items>(json);

        string jsonStack = PlayerPrefs.GetString("InventoryStack");
        // Deserialize the JSON string to the wrapper class
        IntListWrapper wrapper = JsonUtility.FromJson<IntListWrapper>(jsonStack);
        // Access the deserialized list
        List<int> deserializedStackList = wrapper.stackList;


        Inventory.instance.items = deserializedItemsList;

        // Update the stack values in your Items list
        for (int i = 0; i < Mathf.Min(deserializedStackList.Count, deserializedItemsList.Count); i++)
        {
            Inventory.instance.items[i].stack = deserializedStackList[i];
        }

        Inventory.instance.UpdateInventory();
        //***********************************************************************
    }

    private void LoadDataBetweenScenes()
    {

        string jsonEquipment = PlayerPrefs.GetString("SavedEquipment");
        List<Items> deserializedEquipmentList = JsonHelper.FromJson<Items>(jsonEquipment);
        EquipmentManager.instance.equipmentList = deserializedEquipmentList;
        EquipmentManager.instance.RefreshEquipment();



        //**********************FOR INVENTORY***********************************
        string json = PlayerPrefs.GetString("SavedInventory");
        List<Items> deserializedItemsList = JsonHelper.FromJson<Items>(json);

        string jsonStack = PlayerPrefs.GetString("SavedInventoryStack");
        // Deserialize the JSON string to the wrapper class
        IntListWrapper wrapper = JsonUtility.FromJson<IntListWrapper>(jsonStack);
        // Access the deserialized list
        List<int> deserializedStackList = wrapper.stackList;


        Inventory.instance.items = deserializedItemsList;

        // Update the stack values in your Items list
        for (int i = 0; i < Mathf.Min(deserializedStackList.Count, deserializedItemsList.Count); i++)
        {
            Inventory.instance.items[i].stack = deserializedStackList[i];
        }

        Inventory.instance.UpdateInventory();
        //***********************************************************************
    }


    public void GetData()
    {
        string json = PlayerPrefs.GetString("Inventory");
        List<Items> deserializedItemsList = JsonHelper.FromJson<Items>(json);
        Inventory.instance.items = deserializedItemsList;


        player.hitPoints = int.Parse(PlayerPrefs.GetString("HitPoints"));
        player.manaPoints = 50;
        Debug.Log("The loaded hp val" + int.Parse(PlayerPrefs.GetString("HitPoints")));




    }





    public void SaveGame()
    {
        StartCoroutine(InsertSavedUserDataIntoDatabase());
    }

    public void SaveBetweenScene()
    {
        // Serialize equipment items
        string jsonEquipment = JsonHelper.ToJson<Items>(EquipmentManager.instance.equipmentList);
        PlayerPrefs.SetString("SavedEquipment", jsonEquipment);

        // Serialize inventory items
        string jsonInventoryItems = JsonHelper.ToJson<Items>(Inventory.instance.items);
        PlayerPrefs.SetString("SavedInventory", jsonInventoryItems);

        // Serialize inventory stack list
        List<int> stackList = new List<int>();
        foreach (Items item in Inventory.instance.items)
        {
            stackList.Add(item.stack);
        }

        IntListWrapper wrapper = new IntListWrapper
        {
            stackList = stackList
        };

        // Serialize the wrapper class to JSON
        string jsonStack = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString("SavedInventoryStack", jsonStack);
        Debug.Log("Save occured");
    }


    private IEnumerator InsertSavedUserDataIntoDatabase()
    {

        string hasCompanion = "false";

        // Get the current active scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        // Get the Player component from the GameObject
        player = playerObject.GetComponent<Player>();
        // Convert Player position Vector3 to JSON
        string jsonPosition = JsonUtility.ToJson(player.transform.position);

        //Serialize equipment items to save into database
        string jsonEquipment = JsonHelper.ToJson<Items>(EquipmentManager.instance.equipmentList);

        //Serialize inventory items to save into database
        string jsonInventoryItems = JsonHelper.ToJson<Items>(Inventory.instance.items);

        List<int> stackList = new List<int>();
        foreach (Items item in Inventory.instance.items)
        {
            stackList.Add(item.stack);

        }

        // Wrap the list in a serializable class
        IntListWrapper wrapper = new IntListWrapper
        {
            stackList = stackList
        };

        // Serialize the wrapper class to JSON
        string jsonStack = JsonUtility.ToJson(wrapper);



        if (PlayerPrefs.GetString("Companion") == "Yes")
        {
           
            Debug.Log("Found a companion in the scene!");
            hasCompanion = "true";

        }
        



        string UserId = PlayerPrefs.GetString("UserId");
        //HP, Mana, Inventory, EnemyMobs, NPC Companion, Score 
        Debug.Log("This is the player health at the time of saving" + player.hitPoints);
        // Create tasks for setting multiple values
        Task hpTask = DBreference.Child("users").Child(UserId).Child("hp").SetValueAsync((player.hitPoints).ToString());
        Task manaTask = DBreference.Child("users").Child(UserId).Child("mana").SetValueAsync((player.manaPoints).ToString());
        Task inventoryTask = DBreference.Child("users").Child(UserId).Child("inventory").SetValueAsync(jsonInventoryItems);
        Task inventoryStackTask = DBreference.Child("users").Child(UserId).Child("inventoryStack").SetValueAsync(jsonStack);
        Task equipmentTask = DBreference.Child("users").Child(UserId).Child("equipment").SetValueAsync(jsonEquipment);
        Task positionTask = DBreference.Child("users").Child(UserId).Child("position").SetValueAsync(jsonPosition);
        Task savedGameAvailableTask = DBreference.Child("users").Child(UserId).Child("savedgame").SetValueAsync("true");
        Task companionTask = DBreference.Child("users").Child(UserId).Child("companion").SetValueAsync(hasCompanion);
        Task sceneIndex = DBreference.Child("users").Child(UserId).Child("sceneIndex").SetValueAsync(currentSceneIndex);

        // Create an array of tasks to wait for
        Task[] tasksToWaitFor = { hpTask, manaTask, inventoryTask, inventoryStackTask, equipmentTask, positionTask, savedGameAvailableTask, companionTask, sceneIndex };

        // Wait for all tasks to complete
        yield return new WaitUntil(() => Task.WhenAll(tasksToWaitFor).IsCompleted);

        // Check for exceptions in any of the tasks
        if (tasksToWaitFor.Any(task => task.Exception != null))
        {
            foreach (var task in tasksToWaitFor)
            {
                if (task.Exception != null)
                {
                    Debug.LogWarning($"Failed to update data with exception: {task.Exception}");
                }
            }
        }
        else
        {
            // All values are updated successfully
            Debug.Log("Game Saved Successfully");
        }
    }
}
