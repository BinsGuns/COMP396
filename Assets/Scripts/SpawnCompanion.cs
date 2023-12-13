using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCompanion : MonoBehaviour
{
    public GameObject spawnPrefab; // The prefab to spawn
    public GameObject panel;

    private void OnDestroy()
    {
        SpawnNewObject();
    }

    private void SpawnNewObject()
    {
        if (spawnPrefab != null)
        {
            panel.SetActive(true);
            DataManager.instance.SaveBetweenScene();
            // Instantiate a new object at the same position and rotation as the current one
            GameObject newObject = Instantiate(spawnPrefab, transform.position, transform.rotation);

            // Optionally, you can do additional setup for the spawned object here
        }
        else
        {
            Debug.LogWarning("Spawn prefab is not assigned in the inspector.");
        }
    }
}
