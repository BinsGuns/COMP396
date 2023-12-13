using System.Collections;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Skills;

public class PlayerController : MonoBehaviour
{

    [SerializeField] Animator animator;
    public Interactable interactable;

    private TargetLock targetLock;
    [SerializeField] public SpawnProjectile spawnProjectile;

    public float maxDistance = 10f;

    public bool isAttackingMonster = false;

    [Header("Layer Index For Animation")]
    private int baseLayerIndex;
    private int combatLayerIndex;
    private int basicMagicAttackLayerIndex;
    private Player player;


    [Header("Animator Parameters")]
    [Space]
    public string walkForward = "Walk Forward";
    public string runForward = "Run Forward";
    public string stabAttack = "Stab Attack";
    public string hurt = "Take Damage";

    [Header("Skill")]
    private Skills skill;
    private bool isFirstSkillOnCooldown = false;
    private bool isOnCooldown = false;
    public TMP_Text firstSkillCoolDownText;
    private float cooldownTimer = 0f;
    public Button firstSkillButton;

    [Header("Pop Up Message Section")]
    public TMP_Text popupMessage;
    public PanelFader panelFader;
    public float messageDisplayTime = 5f;

    public GameObject campanionTarget;


    // Start is called before the first frame update
    void Start()
    {
        targetLock = GetComponent<TargetLock>();
        player = GetComponent<Player>();
        basicMagicAttackLayerIndex = animator.GetLayerIndex("Basic Magic Attack Layer");
        skill = FindObjectOfType<Skills>();
        Interactable interactable = this.GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EquipmentManager.instance != null)
        {
            spawnProjectile.IsFirePointActiveAndEquipped();
            // Check if the right mouse button is clickedj
            if (Input.GetMouseButtonDown(1) && targetLock.isTargeting == true && EquipmentManager.instance.playercanfire == true)  // 1 = Right Mouse Click
            {
                // Check the distance condition (e.g., if closer than 10 units)
                if (Vector3.Distance(transform.position, targetLock.TheCurrentEnemy().transform.position) < maxDistance)
                {

                    campanionTarget = targetLock.TheCurrentEnemy();
                    isAttackingMonster = true;
                    spawnProjectile.enabled = true;
                    animator.SetLayerWeight(basicMagicAttackLayerIndex, 1);
                    Invoke(nameof(ClosePopUpMessage), messageDisplayTime);

                }
                else
                {
                    popupMessage.text = "Too far from the enemy to attack!";
                    panelFader.showUI();
                    Invoke(nameof(ClosePopUpMessage), messageDisplayTime);
                }


            }


            if (targetLock.isTargeting == false)
            {
                spawnProjectile.enabled = false;
                animator.SetLayerWeight(basicMagicAttackLayerIndex, 0);
                isAttackingMonster = false;
                campanionTarget = null;

            }
        }
        if (player.isTakingDamage == true)
        {
            spawnProjectile.enabled = false;
            animator.SetLayerWeight(basicMagicAttackLayerIndex, 0);
            animator.SetTrigger(hurt);
            spawnProjectile.enabled = true;
            animator.SetLayerWeight(basicMagicAttackLayerIndex, 1);
            player.isTakingDamage = false;

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (EquipmentManager.instance.weaponItem != null)
            {
                useFirstSkill();
                Invoke(nameof(ClosePopUpMessage), messageDisplayTime);
            }


            if (EquipmentManager.instance.weaponItem == null)
            {

                popupMessage.text = "No weapon equipped to use skill";
                panelFader.showUI();
                Invoke(nameof(ClosePopUpMessage), messageDisplayTime);
            }


        }


        // Update cooldown timer if on cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (firstSkillCoolDownText != null)
            {
                firstSkillCoolDownText.text =
                    Mathf.FloorToInt(cooldownTimer).ToString(); // Round down to nearest integer
                // Check if cooldown is over
                if (cooldownTimer <= 0f)
                {
                    // Cooldown is over
                    isOnCooldown = false;

                    // Enable interaction
                    SetInteractable(true);

                    // Hide or update the cooldown text
                    firstSkillCoolDownText.text = ""; // You may want to hide the text or set it to an empty string
                    firstSkillCoolDownText.enabled = false;
                }
            }
        }


        // Check for item pickup
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptItemPickup();
        }



    }

    void AttemptItemPickup()
    {
        if (interactable.inRangeForInteraction == true)
        {
            // Cast a sphere in front of the player to check for triggers
            Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);

            foreach (Collider col in colliders)
            {
                // Check if the collider has the "Item" tag
                if (col.CompareTag("Item"))
                {
                    // Check if the collider has an ItemPickup component
                    ItemPickup itemPickup = col.GetComponent<ItemPickup>();

                    if (itemPickup != null)
                    {
                        // Call the Pickup() method on the item
                        itemPickup.Pickup();
                    }
                }
            }
        }
    }

    public void buttonPressFirstSkill()
    {
        if (EquipmentManager.instance.weaponItem != null)
        {
            useFirstSkill();
            Invoke(nameof(ClosePopUpMessage), messageDisplayTime);
        }


        if (EquipmentManager.instance.weaponItem == null)
        {

            popupMessage.text = "No weapon equipped to use skill";
            panelFader.showUI();
            Invoke(nameof(ClosePopUpMessage), messageDisplayTime);
        }

    }


    public void useFirstSkill()
    {
        if (!isFirstSkillOnCooldown)
        {
            // Access the meteorCircle instance
            MeteorCircleSkill meteorCircleInstance = skill.meteorCircle;

            // Activate the skill
            meteorCircleInstance.ActivateSkill(targetLock.TheCurrentEnemy().transform);

            // Deduct mana
            player.DeductMana(meteorCircleInstance.getManaCost());
            Debug.Log("this is the mana cost" + meteorCircleInstance.getManaCost());

            // Start cooldown
            StartCoroutine(StartFirstSkillCooldown(meteorCircleInstance.getCoolDown()));
            Invoke(nameof(ClosePopUpMessage), messageDisplayTime);

            // Start the cooldown
            StartCooldown(meteorCircleInstance.getCoolDown());

            // Disable interaction during cooldown
            firstSkillButton.interactable = false;
        }
        else
        {
            // Skill is on cooldown, you can add some feedback or just ignore the button press
            popupMessage.text = "Meteor Circle is on cooldown";
            panelFader.showUI();
            Invoke(nameof(ClosePopUpMessage), messageDisplayTime);
        }
    }

    IEnumerator StartFirstSkillCooldown(int coolDownTime)
    {
        isFirstSkillOnCooldown = true;

        // Wait for the cooldown time
        yield return new WaitForSeconds(coolDownTime);

        // Cooldown is over
        isFirstSkillOnCooldown = false;
    }

    public void useSecondSkill()
    {

    }


    public void isPicking()
    {
        animator.SetTrigger("IsPicking");
    }
    private void ClosePopUpMessage()
    {
        panelFader.hideUI();
    }


    private void StartCooldown(float cooldownDuration)
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
        firstSkillCoolDownText.enabled = true;
    }

    private void SetInteractable(bool value)
    {
        // You can set the interactable property of your button here
       firstSkillButton.interactable = value;
    }


    public void addCompanion()
    {
        PlayerPrefs.SetString("Companion", "Yes");
    }

    public Transform getCurrentTarget()
    {
        Transform currentTarget = targetLock.TheCurrentEnemy().transform;
        return currentTarget;
    }
}
