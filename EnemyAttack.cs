using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyData EnemyData;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(EnemyData.Damage);
            }
        }
    }
}