using System;
using System.Collections.Generic;
using Cinemachine;
using Fusion;
using Multiplayer;
using UnityEngine;

public class CharacterMovementMultiplayer : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 5f; // Adjust the speed as needed
    [SerializeField] float rotationSpeed = 500f; // Adjust the speed as needed

    private float gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    public float velocityY;
    public float jumpForce = 8.0f;
    

    Quaternion targetRotation;


    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    private Camera cam;
    [SerializeField] private CinemachineFreeLook _freeLook;
    
   // public CameraController cameraController;
    [SerializeField] Animator animator;

    CharacterController characterController;
    Player player;

    [Header("Animator Parameters")]
    [Space]
    public string movementAmount = "moveAmount";
    public string moveX = "MoveX";
    public string moveZ = "MoveZ";
    public string specialAttack2 = "isAttack02";
    public string notspecialAttack2 = "isNotAttack02";
    public string specialAttack3 = "isAttack03";
    public string notspecialAttack3 = "isNotAttack03";


    [Header("Layer Index For Animation")]
    private int baseLayerIndex;
    private int combatLayerIndex;



    private PlayerController playerController;

    public bool inCombat = false;
    public List<AudioClip> audioClips;
    private AudioSource _audioSource;
    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        baseLayerIndex = animator.GetLayerIndex("Base Layer");
        combatLayerIndex = animator.GetLayerIndex("Combat Layer");
        player = GetComponent<Player>();
        GameObject.Find("EquipmentManager").SetActive(true);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = audioClips[0];
        
        // GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        // for(int i = 0; i < items.Length; i++)
        // {
        //     Debug.Log("ITEM SHOW");
        //     items[i].SetActive(true);
        // }

     
    }

   
    private void PlayWalkSound()
    {
        _audioSource.Play();
    }
    
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
           // cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();  
            GameObject.FindWithTag("Camera").GetComponent<CinemachineFreeLook>().Follow = gameObject.transform;
            GameObject.FindWithTag("Camera").GetComponent<CinemachineFreeLook>().LookAt = gameObject.transform;
        }
        //if(HasStateAuthority == false)
      //      gameObject.transform.GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
      //  PlayerRef player = GetComponent<NetworkObject>().HasStateAuthority
      // GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
      // foreach (GameObject player in players)
      // {
      //     NetworkId id = player.GetComponent<NetworkObject>().Id;
      //     if (GetComponent<NetworkObject>().Id != id)
      //     {
      //         player.gameObject.transform.GetComponentInChildren<CinemachineFreeLook>().gameObject.SetActive(false);
      //     }
      // }
   //  CinemachineFreeLook ca = GameObject.FindGameObjectsWithTag("Player")[0].gameObject.transform.GetComponentInChildren<CinemachineFreeLook>();
    //Debug.Log(ca);

      // {
      //     Debug.Log("SwwwwWPANEED");
      //     cam = Camera.main;
      //     GetComponent<TargetLock>().mainCamera = cam;
      //     GetComponent<TargetLock>().cinemachineFreeLook = GameObject.FindGameObjectWithTag("Camera").GetComponent<CinemachineFreeLook>();
      //     GetComponent<TargetLock>().cinemachineFreeLook.LookAt = gameObject.transform;
      //     GetComponent<TargetLock>().cinemachineFreeLook.Follow = gameObject.transform;
      //     // Camera.GetComponent<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;
      // }
    }
    public override void FixedUpdateNetwork()

    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        
        if (GetInput(out NetworkInputData data))
        {
            // Debug.Log(data.direction);
            data.direction.Normalize();
            //_cc.Move(5*data.direction*Runner.DeltaTime);
            
                 //CLamp the value between 0 and 1 for the animation blend
            float moveAmount = Mathf.Clamp01(Mathf.Abs(data.direction.z) + Mathf.Abs(data.direction.x));

            // Calculate the movement vector
            Vector3 movement = new Vector3(data.direction.x, 0.0f, data.direction.z);
            movement.Normalize(); // Normalize to ensure diagonal movement isn't faster
            
            if (movement.x != 0 || movement.z != 0)
            {
                if (!_audioSource.isPlaying)
                {
                    PlayWalkSound();
                }
            }
            else
            {
                _audioSource.Stop();
            }
            //movement.y = velocityY;

            //var moveDirection = cameraController.PlanarRotation * movement;
            var moveDirection = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) * movement;
            // Calculate the velocity based on the input and moveSpeed
            Vector3 velocity = moveDirection * moveSpeed;

            /*
            // Apply gravity
            velocityY += gravity * gravityMultiplier * Time.deltaTime;

            // Apply the vertical velocity to the object's movement
            velocity.y = velocityY;

           */



            GroundCheck();

            if (isGrounded && velocityY < 0.0f)
            {
                velocityY = -1f;

            }
            else
            {
                // Apply gravity
                velocityY += gravity * gravityMultiplier * Runner.DeltaTime;
            }

            // Apply the vertical velocity to the object's movement
            velocity.y = velocityY;

            if (inCombat == false)
            {
                player.StartHealthRegeneration(true);
                animator.SetLayerWeight(combatLayerIndex, 0);
                if (moveAmount > 0)
                {
                    // Translate the character's position based on the input
                    //transform.Translate(movement * moveSpeed * Time.deltaTime);
                    //transform.position += moveDirection * moveSpeed * Time.deltaTime;

                    characterController.Move(velocity * Runner.DeltaTime);

                    targetRotation = Quaternion.LookRotation(moveDirection);

                }



                //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                animator.SetFloat(movementAmount, moveAmount);
            }


            if (inCombat == true)
            {
                player.StopHealthRegeneration();
                animator.SetLayerWeight(combatLayerIndex, 1);
                if (moveAmount > 0)
                {
                    // Translate the character's position based on the input
                    //transform.Translate(movement * moveSpeed * Time.deltaTime);
                    //transform.position += moveDirection * moveSpeed * Time.deltaTime;

                    characterController.Move(velocity * Runner.DeltaTime);

                    targetRotation = Quaternion.LookRotation(moveDirection);

                }
                

                //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                animator.SetFloat(moveX, data.direction.x);
                animator.SetFloat(moveZ, data.direction.z);
            }


            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);

            

            if (data.direction.y > 0 && isGrounded)
            {

                animator.SetBool("IsJumping", true);
                velocityY = jumpForce;

            }
            else if (isGrounded)
            {
                animator.SetBool("IsJumping", false);
            }

        
        }
        // Get input axes (WASD or arrow keys)
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        
        // Debug.Log(moveHorizontal);
        // Debug.Log(moveVertical);
        //velocityY += gravity * gravityMultiplier * Time.deltaTime;

        

    }


    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }


    public void enteringCombat()
    {
        inCombat = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

}
