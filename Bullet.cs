using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10; // Damage inflicted by the bullet

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the bullet upon hitting the enemy

        }
    }
}