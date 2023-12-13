using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : MonoBehaviour
{
    public Items itemWeapon;
    // Start is called before the first frame update
    void Start()
    {

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

        Inventory.instance.Add(itemWeapon);

        Inventory.instance.UpdateInventory();
        // Force update the inventory UI
        Inventory.instance.UpdateUI();
    }

    [System.Serializable]
    public class IntListWrapper
    {
        public List<int> stackList;
    }

}
