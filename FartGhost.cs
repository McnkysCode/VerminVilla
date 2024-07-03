using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FartGhost : MonoBehaviour
{
    private Transform player;
    public EnemyData Data;
    private Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool Spawning = true;
    [SerializeField] private AnimationClip SpawnAnimation;
    [SerializeField] private float attackRange = 5.0f; // Distance to trigger attack
    [SerializeField] private float attackCooldown = 2.0f; // Cooldown between attacks
    private bool canAttack = true;
    public AnimationClip deathAnim;
    public EnemyHealth health;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            return;
        }

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(awakePause());
    }

    private IEnumerator awakePause()
    {
        animator.SetTrigger("Awake");
        Spawning = true;
        yield return new WaitForSeconds(SpawnAnimation.length);
        Spawning = false;
    }

    private void Update()
    {
        if (player != null && Spawning == false)
        {
            Vector2 direction = player.position - transform.position;
            float distanceToPlayer = direction.magnitude;

            if (distanceToPlayer <= attackRange)
            {
                StartCoroutine(Attack());
            }
            else
            {
                direction.Normalize();
                transform.Translate(direction * Data.Speed * Time.deltaTime);

                if (direction.x != 0f || direction.y != 0f)
                {
                    animator.SetBool("Walking", true);
                    animator.SetFloat("Horizontal", direction.x);

                    if (direction.x > 0f)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                    }
                }
                else
                {
                    animator.SetBool("Walking", false);
                }
            }
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        animator.SetTrigger("Shooting");

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(deathAnim.length);

        // Perform AoE damage
        PerformAoEDamage();

        //// Wait for attack cooldown
        //yield return new WaitForSeconds(attackCooldown);
        //canAttack = true;
    }

    private void PerformAoEDamage()
    {
        // Define the radius of the AoE attack
        float aoeRadius = 3.0f; // Adjust as needed
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {

            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("busting all over the player freaky style");
                    playerHealth.TakeDamage(9999);
                    // Die after attacking
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        rb.gameObject.SetActive(false);
        animator.SetTrigger("Death");
        if (PlayerXp.Instance != null)
        {
            PlayerXp.Instance.EnemyKilled(health.xpGainOnDeath);
        }
        Destroy(gameObject, deathAnim.length);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the AoE range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f); // Adjust the radius to match aoeRadius
    }
}
