using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public EnemyData Data;
    //public AudioSource deathSound; 

    public int xpGainOnDeath = 10;
    private GameObject player;
    public Color hitColor = Color.red;
    private float flashDuration = 0.3f;
    public SpriteRenderer spriteRenderer;
    [SerializeField] public AnimationClip deathAnimation;
    private Animator animator;
    private BoxCollider2D bc2d;

    private int currentHealth;
    private Enemy enemyScript;  // Add reference to the Enemy script

    private void Start()
    {
        // Use health from Data if available, otherwise use maxHealth
        if (Data != null)
        {
            currentHealth = Data.Health;
        }
        else
        {
            currentHealth = maxHealth;
        }

        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        enemyScript = GetComponent<Enemy>();  // Get reference to the Enemy script

        // Debug logs to confirm initialization
        Debug.Log("EnemyHealth initialized. Health set to: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage. Current health: " + currentHealth); // Debug log for damage

        StartCoroutine(FlashSprite(hitColor, flashDuration));

        if (currentHealth <= 0)
        {
            //deathSound.Play();
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        bc2d.enabled = false;

        StartCoroutine(FlashSprite(hitColor, flashDuration));
        yield return new WaitForSeconds(flashDuration);

        // Stop the enemy from moving
        if (enemyScript != null)
        {
            enemyScript.CanMove = false;
        }

        animator.SetTrigger("Death");
        if (PlayerXp.Instance != null)
        {
            PlayerXp.Instance.EnemyKilled(xpGainOnDeath);
        }

        yield return new WaitForSeconds(deathAnimation.length);
        Destroy(gameObject);
    }

    private IEnumerator FlashSprite(Color color, float duration)
    {
        spriteRenderer.color = color;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = Color.white;
    }
}
