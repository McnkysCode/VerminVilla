using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float duration = 10f;
    public float bulletCooldownMultiplier = 0.5f; // Makes bullets shoot faster
    public int bulletDamageBonus = 10; // Increases bullet damage

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Attack playerAttack = other.GetComponent<Attack>();
            if (playerAttack != null)
            {
                playerAttack.ApplyPowerUp(bulletCooldownMultiplier, bulletDamageBonus, duration);
            }
            Destroy(gameObject); // Destroy the power-up upon picking it up
        }
    }
}