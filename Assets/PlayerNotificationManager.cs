using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotificationManager : MonoBehaviour
{
    public List<GameObject> EnemyPrefab;
    public GameObject EquipmentManager;

    private bool _objectSpawned = false;
    // Update is called once per frame
    void Update()
    {

        if (!GameObject.FindGameObjectWithTag("Player") && !_objectSpawned)
        {
            Debug.Log("SPAWWMm");
            EnemyPrefab[0].SetActive(true);
            EquipmentManager.SetActive(true);
            _objectSpawned = true;
        }
    }
}
