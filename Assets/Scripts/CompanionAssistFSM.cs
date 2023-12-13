using UnityEngine;
using UnityEngine.AI;


public class CompanionAssistFSM : MonoBehaviour
{
   
    public Transform player;
    public float followDistance = 1f;
    public float attackDistance = 1f;
    public float attackCooldown = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private float attackTimer = 0f;
    public GameObject enemyTarget;

    public bool alreadyAttacking = false;

    private PlayerController playerController; // Reference to the PlayerController script

    public enum CompanionState
    {
        Follow,
        Chase,
        Attack,
        Idle
    }

    public CompanionState currentState = CompanionState.Follow;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Get the PlayerController script
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        switch (currentState)
        {
            case CompanionState.Follow:
                FollowPlayer();
                break;
            case CompanionState.Attack:
                AttackTarget();
                break;
            case CompanionState.Chase:
                Chase();
                break;
            case CompanionState.Idle:
                Idling();
                break;
        }

        // Make the companion face the direction of the targeted monster
        if (currentState == CompanionState.Attack)
        {
            FaceTarget(player.position);
        }

        if (playerController.isAttackingMonster == true && alreadyAttacking == false)
        {
            currentState = CompanionState.Attack;
        }

       
    }

    void FollowPlayer()
    {
        alreadyAttacking = false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > followDistance)
        {
            agent.SetDestination(player.position);
            animator.SetBool("Run Forward", true);
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetBool("Run Forward", false);
        }

        
    }

    void AttackTarget()
    {

        enemyTarget = playerController.campanionTarget;
        if (enemyTarget == null)
        {
            currentState = CompanionState.Follow;
            return;
        }

        alreadyAttacking = true;
        float distanceToTarget = Vector3.Distance(transform.position, enemyTarget.transform.position);

        if (distanceToTarget > attackDistance)
        {
            currentState = CompanionState.Chase;
        }

        if (attackTimer <= 0f)
        {
            animator.SetTrigger("Slice Attack");
            DealDamageToTarget();

            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void Chase()
    {
        float distanceToEnemy = Vector3.Distance(transform.position, enemyTarget.transform.position);

        if (distanceToEnemy <= attackDistance)
        {
            animator.SetBool("Run Forward", false);
            currentState = CompanionState.Attack;
        }
        else
        {
            agent.SetDestination(enemyTarget.transform.position);
            animator.SetBool("Run Forward", true);
        }
    }



    void DealDamageToTarget()
    {
        // Get the target from the player's script
        Transform target = playerController.getCurrentTarget();

        if (target != null)
        {
            target.GetComponent<Enemy>().TakeDamage(10);
        }
    }


    public void Idling()
    {
        alreadyAttacking = false;
        currentState = CompanionState.Idle;
        animator.SetBool("Run Forward", false);
    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
