using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Button movementButton;
    public Button itemsButton;
    public Button objectivesButton;
    public Button generalButton;

    public GameObject movementPanel;
    public GameObject itemsPanel;
    public GameObject objectivesPanel;
    public GameObject generalPanel;

    public Button ready;
    public GameObject readyPanel;
    public GameObject wall;
    public GameObject congratsPanel;

    public CinemachineFreeLook freeLookCamera;

    public string tutorialTag = "Tutorial";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // Check if any tutorial panel is open
        bool anyPanelOpen = movementPanel.activeSelf || itemsPanel.activeSelf || objectivesPanel.activeSelf || generalPanel.activeSelf;

        if (anyPanelOpen)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 0;
            freeLookCamera.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            // Unfreeze camera movement
            freeLookCamera.m_XAxis.m_MaxSpeed = 450;
            freeLookCamera.m_YAxis.m_MaxSpeed = 4;
        }

        GameObject[] tutorialObjects = GameObject.FindGameObjectsWithTag(tutorialTag);

        bool allButtonsDisabled = true;

        foreach (GameObject tutorialObject in tutorialObjects)
        {
            // Assuming each tutorial object has a Button component
            Button tutorialButton = tutorialObject.GetComponent<Button>();

            if (tutorialButton != null && tutorialButton.interactable)
            {
                allButtonsDisabled = false;
                break;
            }
        }

        if (allButtonsDisabled)
        {
            Debug.Log("All tutorial buttons with tag '" + tutorialTag + "' are not interactable.");
            if (readyPanel != null)
            {
                readyPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Ready Panel not assigned to the script.");
            }
        }
        else
        {
//            Debug.Log("Not all tutorial buttons with tag '" + tutorialTag + "' are disabled.");
        }


        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (enemy == null)
        {
            // No enemy found, enable congrats panel
            if (congratsPanel != null)
            {
                congratsPanel.SetActive(true);

                DataManager.instance.SaveBetweenScene();
                // Wait for 10 seconds
                StartCoroutine(WaitAndLoadNextScene(10f));
            }
            else
            {
                Debug.LogWarning("Congrats Panel not assigned to the script.");
            }
        }


    }

    public void MovementTutorial()
    {
        SetTutorialState(movementButton, movementPanel, false, true);
    }

    public void ItemsTutorial()
    {
        SetTutorialState(itemsButton, itemsPanel, false, true);
    }

    public void ObjectivesTutorial()
    {
        SetTutorialState(objectivesButton, objectivesPanel, false, true);
    }

    public void GeneralTutorial()
    {
        SetTutorialState(generalButton, generalPanel, false, true);
    }


    public void ReadyTutorial()
    {
        Destroy(wall);
        Destroy(readyPanel);
    }

    private void SetTutorialState(Button button, GameObject panel, bool buttonInteractable, bool panelActive)
    {
        button.interactable = buttonInteractable;
        panel.SetActive(panelActive);
    }

    IEnumerator WaitAndLoadNextScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Load the next scene
        EquipmentManager.instance.UnequipWeapon();
        SceneManager.LoadScene(2);

    }


}
