using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveButtonManager : MonoBehaviour
{


    public void SaveGame()
    {
        DataManager.instance.SaveGame();
    }
}
