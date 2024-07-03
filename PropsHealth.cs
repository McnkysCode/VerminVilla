using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PropsHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public EnemyData Data;
    //public AudioSource deathSound; // Reference to the AudioSource component

    //public Slider healthSlider; // Reference to the health slider UI element
    public int xpGainOnDeath = 10;
    private GameObject player;
    public Color hitColor = Color.red;
    private float flashDuration = 0.3f;
    public SpriteRenderer spriteRenderer;
    [SerializeField] public AnimationClip deathAnimation;
    public BroomBehavior enemy;
    private Animator animator;
    private BoxCollider2D bc2d;


    private void Start()
    {
        enemy = GetComponent<BroomBehavior>();
        Data.Health = maxHealth;
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    public void TakeDamage(int damage)
    {
        Data.Health -= damage;
        //UpdateHealthBar();
        StartCoroutine(FlashSprite(hitColor, flashDuration));

        if (Data.Health <= 0)
        {
            //deathSound.Play();
            StartCoroutine(Die());
        }
    }
    private IEnumerator Die()
    {
        // Perform death behavior, such as playing death animation, spawning particles, etc.
        bc2d.enabled = false;
        enemy.enabled = false;
        StartCoroutine(FlashSprite(hitColor, flashDuration));
        yield return new WaitForSeconds(flashDuration);
        animator.SetTrigger("Death");
        if (PlayerXp.Instance != null)
        {
            PlayerXp.Instance.EnemyKilled(xpGainOnDeath);
        }

        yield return new WaitForSeconds(deathAnimation.length);
        enemy.enabled = true;
        Destroy(gameObject);
    }

    //void UpdateHealthBar()
    //{
    //    if (healthSlider != null)
    //    {
    //        healthSlider.value = (float)Data.Health / maxHealth;
    //    }
    //}

    private IEnumerator FlashSprite(Color color, float duration)
    {
        // Change sprite color to the specified color
        spriteRenderer.color = color;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset sprite color
        spriteRenderer.color = Color.white;
    }
}
