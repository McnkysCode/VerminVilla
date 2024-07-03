using UnityEngine;
using System.Collections;

public class BroomBehavior : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float attackRange = 5f;
    public float attackCooldown = 1f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;

    private Rigidbody2D rb;
    private Vector2 roamDirection;
    private Transform player;
    private bool isAttacking = false;
    private float lastAttackTime;
    private ParticleSystem[] particleEffects;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        particleEffects = GetComponentsInChildren<ParticleSystem>();
        ChooseNewRoamDirection();
        StartCoroutine(DestroyAfterTime(20f));
    }

    void Update()
    {
        if (isAttacking)
        {
            if (Time.time - lastAttackTime > attackCooldown)
            {
                isAttacking = false;
                ChooseNewRoamDirection();
            }
        }
        else
        {
            Roam();

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
        }
    }

    void Roam()
    {
        rb.velocity = roamDirection * roamSpeed;
    }

    void ChooseNewRoamDirection()
    {
        float angle = Random.Range(0f, 360f);
        roamDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttacking)
        {
            roamDirection = Vector2.Reflect(roamDirection, collision.contacts[0].normal);
        }
    }

    void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        rb.velocity = Vector2.zero;

        // Play particle effects
        foreach (var particle in particleEffects)
        {
            particle.Play();
        }

        // Knock the player back
        Vector2 knockbackDirection = (player.position - transform.position).normalized;
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            Invoke(nameof(ResetPlayerVelocity), knockbackDuration);
        }

        // After attacking, resume roaming after a cooldown period
        Invoke(nameof(ResumeRoaming), attackCooldown);
    }

    void ResetPlayerVelocity()
    {
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;
        }
    }

    void ResumeRoaming()
    {
        isAttacking = false;
        ChooseNewRoamDirection();
    }

    // Coroutine to destroy the GameObject after a set time
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
