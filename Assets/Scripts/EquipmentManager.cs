using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{

    [Header("Weapon Spawn Settings")]
    [SerializeField] private Vector3 spawnPositionOffset = new Vector3(0.00335f, -0.00062f, -0.00035f);
    [SerializeField] private Vector3 spawnRotationEuler = new Vector3(-62.961f, 114.203f, -27.48f);

    [Header("Weapon Slot")]
    [Space]
    public Image defaultWeaponIcon;
    public Image weaponIcon;
    public bool weaponSlotOccupied = false;
    public Items weaponItem;
    [Space]
    [Header("Helmet Slot")]
    [Space]
    public Image defaultHelmetIcon;
    public Image helmetIcon;
    public bool helmetSlotOccupied = false;
    Items helmetItem;
    [Space]
    [Header("Armor Slot")]
    [Space]
    public Image defaultArmorIcon;
    public Image armorIcon;
    public bool armorSlotOccupied = false;
    Items armorItem;
    [Space]
    [Header("Left Glove Slot")]
    [Space]
    public Image defaultLeftGloveIcon;
    public Image leftGloveIcon;
    public bool leftGloveSlotOccupied = false;
    Items leftGloveItem;
    [Space]
    [Header("Right Glove Slot")]
    [Space]
    public Image defaultRightGloveIcon;
    public Image rightGloveIcon;
    public bool rightGloveSlotOccupied = false;
    Items rightGloveItem;
    [Space]
    [Header("Boot Slot")]
    [Space]
    public Image defaultBootIcon;
    public Image BootIcon;
    public bool bootSlotOccupied = false;
    Items bootItem;





    public GameObject spawnItem;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public List<Items> equipmentList = new List<Items>(); //list of equipment;

    public static EquipmentManager instance;

    private Animator playerAnimator;
    private Transform rightHand;
    private GameObject spawnedWeaponInstance;

    public bool playercanfire = false;
    private bool playerSpawn = false;
    private void Awake()
    {
        // Ensure only one instance of EquipmentManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (!playerSpawn && GameObject.FindWithTag("Player"))
        {
            playerSpawn = true;
            instantiatePlayer();
        }
    }

    void Start()
    {



        // Find the GameObject with the Animator component (replace "Player" with the correct tag or name)
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            // Get the Animator component from the player GameObject
            playerAnimator = playerObject.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                // Example: Find bones by name (adjust these names based on your actual rig)
                rightHand = FindBoneByName(playerObject, "finger.R");
            }
            else
            {
                Debug.LogError("Animator component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure the player has the correct tag or name.");
        }
    }

    private void instantiatePlayer()
    {
        
        // Find the GameObject with the Animator component (replace "Player" with the correct tag or name)
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            // Get the Animator component from the player GameObject
            playerAnimator = playerObject.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                // Example: Find bones by name (adjust these names based on your actual rig)
                rightHand = FindBoneByName(playerObject, "finger.R");
            }
            else
            {
                Debug.LogError("Animator component not found on the player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure the player has the correct tag or name.");
        }
    }

    Transform FindBoneByName(GameObject playerObject, string boneName)
    {
        Transform[] allBones = playerObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform bone in allBones)
        {
            if (bone.name == boneName)
            {
                return bone;
            }
        }

        Debug.LogError($"Bone named {boneName} not found.");
        return null;
    }



    public void newSceneEquipmentRefresh()
    {
        //The issue that mad me mad when trying to equip weapons after tut

        if (defaultWeaponIcon == null)
        {
            // Find the GameObject with the name "weaponslot"
            GameObject weaponSlotObject = GameObject.FindGameObjectWithTag("WeaponSlotBaseIcon");

            if (weaponSlotObject != null)
            {
                // Get the child Image using GetComponentInChildren with a name filter
                Image icon = weaponSlotObject.GetComponent<Image>();

                if (icon != null)
                {
                    // Now you can use the first Image component found
                    defaultWeaponIcon = icon;


                }
                else
                {
                    Debug.LogWarning("Image component not found on weapon slot.");
                }
            }


            GameObject weaponOccupiedSlotObject = GameObject.FindGameObjectWithTag("WeaponSlotOccupiedIcon");
            if (weaponOccupiedSlotObject != null)
            {
                // Get the child Image using GetComponentInChildren with a name filter
                Image icons = weaponOccupiedSlotObject.GetComponent<Image>();

                if (icons != null)
                {
                    // Now you can use the first Image component found
                    weaponIcon = icons;


                }
                else
                {
                    Debug.LogWarning("Image component not found on weapon slot.");
                }
            }
        }


        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            // Get the Animator component from the player GameObject
            playerAnimator = playerObject.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                // Example: Find bones by name (adjust these names based on your actual rig)
                rightHand = FindBoneByName(playerObject, "finger.R");
            }
            else
            {
                Debug.LogError("Animator component not found on the player GameObject.");
            }
        }


    }



    public void EquipWeapon(Items newItem)
    {

        if (newItem)
        {
            Debug.Log("Made it here");
            if (weaponSlotOccupied == true)
            {
                RemoveEquipment(weaponItem);
                ClearWeaponSlot();
                Inventory.instance.Add(newItem);

                // Disable the UI.Image component
                if (weaponIcon != null)
                {
                    weaponIcon.enabled = false;
                }

                AddEquipment(newItem);
                weaponItem = newItem;
                // Enable the UI.Image component with the new item
                if (weaponItem != null && weaponIcon != null)
                {
                    weaponIcon.sprite = weaponItem.icon;
                    weaponIcon.enabled = true;
                    defaultWeaponIcon.enabled = false;
                }
                // Instantiate a new GameObject associated with the item
                spawnedWeaponInstance = Instantiate(newItem.theItem, rightHand.position, rightHand.rotation);

                

                // Set the item as a child of the right hand
                spawnedWeaponInstance.transform.parent = rightHand.transform;
                spawnedWeaponInstance.transform.localPosition = spawnPositionOffset;  // Use the serialized field value for position
                spawnedWeaponInstance.transform.localRotation = Quaternion.Euler(spawnRotationEuler); // Use the serialized field value for rotation



            }
            else
            {

                AddEquipment(newItem);

                weaponItem = newItem;

                weaponIcon.sprite = weaponItem.icon;
                weaponIcon.enabled = true;
                defaultWeaponIcon.enabled = false;

                // Instantiate a new GameObject associated with the item
                spawnedWeaponInstance = Instantiate(newItem.theItem, rightHand.position, rightHand.rotation);

                // Set the item as a child of the right hand
                spawnedWeaponInstance.transform.parent = rightHand.transform;
                spawnedWeaponInstance.transform.localPosition = spawnPositionOffset;  // Use the serialized field value for position
                spawnedWeaponInstance.transform.localRotation = Quaternion.Euler(spawnRotationEuler); // Use the serialized field value for rotation



            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        else
        {
            Debug.Log("Not wokring sigh");
        }
    }


    //*************************************CHECK IF THE WEAPON EQUIPPED IS FIREPOINT*******************************************
    /**Original
    public bool IsFirePointEquipped(GameObject gameObject)
    {
        string originalString = gameObject.name;
        string modifiedString = originalString.Replace("(Clone)", "");

        if (weaponItem.theItem.name == modifiedString)
        {

            playercanfire = true;
            return true;
        }
        else
        {
            return false;
        }

    }
    **/
    public bool IsFirePointEquipped(GameObject gameObject)
    {
        if (weaponItem != null && weaponItem.theItem != null)
        {
            string originalString = gameObject.name;
            string modifiedString = originalString.Replace("(Clone)", "");

            if (weaponItem.theItem.name == modifiedString)
            {
                playercanfire = true;
                return true;
            }
        }

        // If weaponItem or weaponItem.theItem is null, or the names don't match
        playercanfire = false;
        return false;
    }


    //**************************************************************************************************************************








    public void EquipHelmet(Items newItem)
    {
        if (newItem)
        {
            // Check if the slot is occupied
            if (helmetSlotOccupied)
            {
                // Remove existing equipment from the list and inventory
                RemoveEquipment(helmetItem);
                ClearHelmetSlot();
                Inventory.instance.Add(newItem);

                // Add the new equipment to the list
                AddEquipment(newItem);
                helmetItem = newItem;

                // Update the UI
                helmetIcon.sprite = helmetItem.icon;
                helmetIcon.enabled = true;
                defaultHelmetIcon.enabled = false;
            }
            else
            {
                // Add the new equipment to the list
                AddEquipment(newItem);
                helmetItem = newItem;

                // Update the UI
                helmetIcon.sprite = helmetItem.icon;
                helmetIcon.enabled = true;
                defaultHelmetIcon.enabled = false;
            }

            // Invoke the callback if it's set
            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void EquipArmor(Items newItem)
    {
        if (newItem)
        {
            if (armorSlotOccupied)
            {
                RemoveEquipment(armorItem);
                ClearArmorSlot();
                Inventory.instance.Add(newItem);

                AddEquipment(newItem);
                armorItem = newItem;

                armorIcon.sprite = armorItem.icon;
                armorIcon.enabled = true;
                defaultArmorIcon.enabled = false;
            }
            else
            {
                AddEquipment(newItem);
                armorItem = newItem;

                armorIcon.sprite = armorItem.icon;
                armorIcon.enabled = true;
                defaultArmorIcon.enabled = false;
            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void EquipLeftGlove(Items newItem)
    {
        if (newItem)
        {
            if (leftGloveSlotOccupied)
            {
                RemoveEquipment(leftGloveItem);
                ClearLeftGloveSlot();
                Inventory.instance.Add(newItem);

                AddEquipment(newItem);
                leftGloveItem = newItem;

                leftGloveIcon.sprite = leftGloveItem.icon;
                leftGloveIcon.enabled = true;
                defaultLeftGloveIcon.enabled = false;
            }
            else
            {
                AddEquipment(newItem);
                leftGloveItem = newItem;

                leftGloveIcon.sprite = leftGloveItem.icon;
                leftGloveIcon.enabled = true;
                defaultLeftGloveIcon.enabled = false;
            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void EquipRightGlove(Items newItem)
    {
        if (newItem)
        {
            if (rightGloveSlotOccupied)
            {
                RemoveEquipment(rightGloveItem);
                ClearRightGloveSlot();
                Inventory.instance.Add(newItem);

                AddEquipment(newItem);
                rightGloveItem = newItem;

                rightGloveIcon.sprite = rightGloveItem.icon;
                rightGloveIcon.enabled = true;
                defaultRightGloveIcon.enabled = false;
            }
            else
            {
                AddEquipment(newItem);
                rightGloveItem = newItem;

                rightGloveIcon.sprite = rightGloveItem.icon;
                rightGloveIcon.enabled = true;
                defaultRightGloveIcon.enabled = false;
            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void EquipBoot(Items newItem)
    {
        if (newItem)
        {
            if (bootSlotOccupied)
            {
                RemoveEquipment(bootItem);
                ClearBootSlot();
                Inventory.instance.Add(newItem);

                AddEquipment(newItem);
                bootItem = newItem;

                BootIcon.sprite = bootItem.icon;
                BootIcon.enabled = true;
                defaultBootIcon.enabled = false;
            }
            else
            {
                AddEquipment(newItem);
                bootItem = newItem;

                BootIcon.sprite = bootItem.icon;
                BootIcon.enabled = true;
                defaultBootIcon.enabled = false;
            }

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void UnequipWeapon()
    {
        if (weaponItem)
        {
            GameObject projectileManager = GameObject.Find("ProjectileManager");
            SpawnProjectile spawnProjectile = projectileManager.GetComponent<SpawnProjectile>();
            spawnProjectile.firePointEquipped = false;

            Debug.Log("Unequipping Weaponing");

            // Check if spawnedWeaponInstance is not null before accessing it
            if (spawnedWeaponInstance != null)
            {
                Destroy(spawnedWeaponInstance);
                spawnedWeaponInstance = null; // Set it to null after destroying
            }

            RemoveEquipment(weaponItem);
            Inventory.instance.Add(weaponItem);
            ClearWeaponSlot();
            weaponSlotOccupied = false;
            playercanfire = false;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void UnequipHelmet()
    {
        if (helmetItem)
        {
            RemoveEquipment(helmetItem);
            Inventory.instance.Add(helmetItem);
            ClearHelmetSlot();
            helmetSlotOccupied = false;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void UnequipArmor()
    {
        if (armorItem)
        {
            RemoveEquipment(armorItem);
            Inventory.instance.Add(armorItem);
            ClearArmorSlot();
            armorSlotOccupied = false;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void UnequipLeftGlove()
    {
        if (leftGloveItem)
        {
            RemoveEquipment(leftGloveItem);
            Inventory.instance.Add(leftGloveItem);
            ClearLeftGloveSlot();
            leftGloveSlotOccupied = false;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void UnequipRightGlove()
    {
        if (rightGloveItem)
        {
            RemoveEquipment(rightGloveItem);
            Inventory.instance.Add(rightGloveItem);
            ClearRightGloveSlot();
            rightGloveSlotOccupied = false;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }

    public void UnequipBoot()
    {
        if (bootItem)
        {
            RemoveEquipment(bootItem);
            Inventory.instance.Add(bootItem);
            ClearBootSlot();
            bootSlotOccupied = false;

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
    }





    public void ClearWeaponSlot()
    {
        weaponItem = null;

        weaponIcon.sprite = null;
        weaponIcon.enabled = false;
        defaultWeaponIcon.enabled = true;
    }


    public void ClearHelmetSlot()
    {
        helmetItem = null;

        helmetIcon.sprite = null;
        helmetIcon.enabled = false;
        defaultHelmetIcon.enabled = true;
    }

    public void ClearArmorSlot()
    {
        armorItem = null;

        armorIcon.sprite = null;
        armorIcon.enabled = false;
        defaultArmorIcon.enabled = true;
    }

    public void ClearLeftGloveSlot()
    {
        leftGloveItem = null;

        leftGloveIcon.sprite = null;
        leftGloveIcon.enabled = false;
        defaultLeftGloveIcon.enabled = true;
    }

    public void ClearRightGloveSlot()
    {
        rightGloveItem = null;

        rightGloveIcon.sprite = null;
        rightGloveIcon.enabled = false;
        defaultRightGloveIcon.enabled = true;
    }

    public void ClearBootSlot()
    {
        bootItem = null;

        BootIcon.sprite = null;
        BootIcon.enabled = false;
        defaultBootIcon.enabled = true;
    }


    public void RemoveEquipment(Items item)
    {
        if (item)
        {
            List<Items> itemsToRemove = new List<Items>();

            foreach (Items equipment in equipmentList)
            {
                if (equipment.itemType == item.itemType)
                {
                    itemsToRemove.Add(equipment);
                }
            }

            foreach (Items equipment in itemsToRemove)
            {
                equipmentList.Remove(equipment);
            }
        }
    }


    public void RefreshEquipment()
    {
        if (equipmentList != null)
        {
            // Create a copy of the equipment list
            List<Items> equipmentCopy = new List<Items>(equipmentList);

            // Clear all equipment slots
            ClearWeaponSlot();
            ClearHelmetSlot();
            ClearArmorSlot();
            ClearLeftGloveSlot();
            ClearRightGloveSlot();
            ClearBootSlot();

            // Loop through the copied equipment list and equip items in their respective slots
            foreach (Items equipment in equipmentCopy)
            {
                switch (equipment.itemType)
                {
                    case ItemType.Weapon:
                        EquipWeapon(equipment);
                        break;
                    case ItemType.Helmet:
                        EquipHelmet(equipment);
                        break;
                    case ItemType.Armor:
                        EquipArmor(equipment);
                        break;
                    case ItemType.LeftGlove:
                        EquipLeftGlove(equipment);
                        break;
                    case ItemType.RightGlove:
                        EquipRightGlove(equipment);
                        break;
                    case ItemType.Boots:
                        EquipBoot(equipment);
                        break;
                }
            }
        }
    }


    public void AddEquipment(Items item)
    {


        if (item)
        {
            equipmentList.Add(item);
        }

    }
}

