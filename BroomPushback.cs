using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomPushback : MonoBehaviour
{
    public GameObject broomPrefab;         
    public Transform playerTransform;      
    public float pushForce;           
    public float broomLifetime;       
    public float broomInterval;     
    public float broomMaxRange;    
    public float initialDelay;        

    private bool canSpawnBroom = true;

    void Start()
    {
        StartCoroutine(InitialDelayCoroutine());
    }

    IEnumerator InitialDelayCoroutine()
    {
        // Wait for the initial delay before starting the broom spawn coroutine
        yield return new WaitForSeconds(initialDelay);
        StartCoroutine(SpawnBroomCoroutine());
    }

    IEnumerator SpawnBroomCoroutine()
    {
        while (true)
        {
            if (canSpawnBroom)
            {
                if (IsEnemyInRange())
                {
                    // Instantiate the broom at the player's position
                    GameObject broom = Instantiate(broomPrefab, playerTransform.position, Quaternion.identity);

                    // Start the broom pushing coroutine
                    StartCoroutine(BroomPushCoroutine(broom));

                    // Destroy the broom after its lifetime expires
                    Destroy(broom, broomLifetime);

                    canSpawnBroom = false; // Prevent spawning another broom immediately
                }
            }

            // Wait for the next interval
            yield return new WaitForSeconds(broomInterval);
            canSpawnBroom = true; // Allow spawning another broom
        }
    }

    IEnumerator BroomPushCoroutine(GameObject broom)
    {
        // Push enemies once when the broom spawns
        PushEnemiesInRange(broom);

        
        yield return new WaitForSeconds(broomLifetime);
    }

    void PushEnemiesInRange(GameObject broom)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                Vector2 pushDirection = (enemyRigidbody.position - (Vector2)playerTransform.position).normalized;
                float distance = Vector2.Distance(playerTransform.position, enemyRigidbody.position);

                if (distance <= broomMaxRange)
                {
                    enemyRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    bool IsEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distance <= broomMaxRange)
            {
                return true;
            }
        }
        return false;
    }

    public void UpgradeBroomInterval(float newInterval)
    {
        broomInterval = newInterval;
    }
}
