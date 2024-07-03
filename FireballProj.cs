using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProj : MonoBehaviour
{
    public int directDamage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("PlayerFireball") && other.CompareTag("Enemy"))
        {
            Debug.Log("PlayerFireball hit an Enemy");
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(directDamage);
            }
            Destroy(gameObject);
        }
        else if (CompareTag("EnemyFireball") && other.CompareTag("Player"))
        {
            Debug.Log("EnemyFireball hit the Player");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(directDamage);
            }
            Destroy(gameObject);
        }

    }
}
