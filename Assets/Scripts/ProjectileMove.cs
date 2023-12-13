using System.Collections;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{

    public float speed;
    public float firerate;
    public GameObject targetObject;
    public GameObject explosionPrefab;
    public float destroyDelay = 30.0f; // 30 seconds 


    void Start()
    {
        StartCoroutine(DestroyAfterDelay(destroyDelay));

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            TargetLock targetLock = player.GetComponent<TargetLock>();

            if (targetLock != null)
            {

                targetObject = targetLock.TheCurrentEnemy();
            }
            else
            {
                Debug.LogError("AnotherScript not found on the other object.");
            }
        }
        else
        {
            Debug.LogError("OtherObject not found.");
        }
    }

    void Update()
    {
        if (speed != 0 && targetObject != null)
        {
            // Calculate the direction towards the target
            Vector3 targetEnemy = targetObject.transform.position;

            //Allow the projectile to float a little above ground
            targetEnemy.y += 1;

            Vector3 direction = (targetEnemy - transform.position).normalized;

            // Move the projectile in the calculated direction
            transform.position += direction * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed or Target not assigned");
        }
    }


    public void SetTarget(GameObject newTarget)
    {
        targetObject = newTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Instantiate explosionPrefab at the collision point
            Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);

            // Destroy the projectile
            Destroy(gameObject);
        }
        // Destroy the projectile
        Destroy(gameObject);
    }


    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // After the delay, destroy the GameObject
        Destroy(gameObject);
    }


}






