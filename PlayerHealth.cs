using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private bool isInvincible = false;
    public AudioSource damageSound; 
    private Rigidbody2D rb;
    public Color hitColor = Color.yellow;
    public float flashDuration = 0.6f;
    public float invincibilityDuration = 1.6f;
    public SpriteRenderer playerSpriteRenderer;
    [SerializeField] public AnimationClip deathAnimation;
    private Animator animator;
    [SerializeField] private PlayableDirector PD;
    private Movement movement;
    private Attack attack;
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        UnfreezePlayer();
    }
    private void UnfreezePlayer()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        movement.enabled = true;
        attack.enabled = true;
    }
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            damageSound.Play();
            StartCoroutine(FlashSprite(hitColor, flashDuration));

            if (currentHealth <= 0)
            {
                 rb.constraints = RigidbodyConstraints2D.FreezeAll;
                 movement.enabled = false;
                 attack.enabled = false;

                StartCoroutine(Die());
            }
            else
            {
                StartCoroutine(SetInvincibility(true));
            }
            FindAnyObjectByType<PlayerHealthBar>().UpdateHealthBar(currentHealth);
        }
    }

    private IEnumerator FlashSprite(Color color, float duration)
    {
        playerSpriteRenderer.color = color;

        yield return new WaitForSeconds(duration);

        playerSpriteRenderer.color = Color.white;
    }

    private IEnumerator SetInvincibility(bool invincible)
    {
        isInvincible = invincible;

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }
  

    private IEnumerator Die()
    {
        rb.isKinematic = true;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(deathAnimation.length);
        Cursor.lockState = CursorLockMode.None;
        PD.Play();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}