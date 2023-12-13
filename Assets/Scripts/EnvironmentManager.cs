using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject bridge;
    public string nextSceneName = "PolyTerrainFinalLevel"; // Set the name of your next scene
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            Debug.Log("Enemies exist in the scene.");
        }
        if (enemies.Length == 1)
        {
            Debug.Log("No enemies found in the scene.");
            bridge.SetActive(true);
        }


        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.newSceneEquipmentRefresh();
        }

        // Check if a GameObject with the "end level" tag is active
        GameObject endLevelObject = GameObject.FindWithTag("EndLevel");

        if (endLevelObject != null && endLevelObject.activeSelf)
        {
            // Invoke the LoadNextScene method after a delay of 5 seconds
            Invoke("LoadNextScene", 5f);
        }
    }



    void LoadNextScene()
    {
        // You can add additional logic here before loading the next scene if needed
        // For example, save player progress, update scores, etc.

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

}
