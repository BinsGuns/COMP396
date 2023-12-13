using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelScript : MonoBehaviour
{
    public GameObject panel;
    private void OnDestroy()
    {
        panel.SetActive(true);
    }
}
