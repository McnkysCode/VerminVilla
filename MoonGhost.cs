using System.Collections;
using UnityEngine;

public class MoonGhost : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float detectionRange = 5.0f;
    public GameObject projectilePrefab;
    public Transform playerTransform;
    private Vector2 roamDirection;
    public float fireRate = 2.0f;
    public float projectileLifetime = 3.0f;
    private Animator animator;
    public int Health = 20;
    private Rigidbody2D rb;
    private float nextFireTime;
    private BoxCollider2D bc2d;
    public GameObject Projectile;
    public float sightRange = 10f;
    public float attackRange = 5f;
    public float maintainDistance = 7f;
    public float moveSpeed = 2f;
    public float roamRadius = 5f;
    public int xpGainOnDeath = 10;
    public Color hitColor = Color.red;
    private float flashDuration = 0.3f;
    private Vector2 roamPosition;
    private State currentState;
    [SerializeField] private AnimationClip SpawnAnimation;
    [SerializeField] public AnimationClip deathAnimation;
    private SpriteRenderer spriteRenderer;
    private bool Spawning = true;
    public EnemyData Data;

    private enum State
    {
        Roaming,
        Seeking,
        Attacking,
        Dead
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextFireTime = Time.time;
        ChooseNewRoamDirection();
        currentState = State.Roaming;
        roamPosition = GetRoamingPosition();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        GameObject player = GameObject.FindWithTag("Player");
        StartCoroutine(awakePause());

        if (player != null)
        {
            // Get the Transform component of the player
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the tag 'Player'.");
        }
    }

    private IEnumerator awakePause()
    {
        animator.SetTrigger("Awake");
        Spawning = true;
        yield return new WaitForSeconds(SpawnAnimation.length);
        Spawning = false;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        Roam();
        CheckPlayerDistance();
        switch (currentState)
        {
            case State.Roaming:
                Roam();
                if (PlayerInSightRange())
                {
                    currentState = State.Seeking;
                }
                break;

            case State.Seeking:
                SeekPlayer();
                if (PlayerInAttackRange())
                {
                    currentState = State.Attacking;
                }
                else if (!PlayerInSightRange())
                {
                    currentState = State.Roaming;
                }
                break;

            case State.Attacking:
                MaintainDistance();
                if (!PlayerInAttackRange())
                {
                    currentState = State.Seeking;
                }
                else if (!PlayerInSightRange())
                {
                    currentState = State.Roaming;
                }
                break;
        }
    }

    private bool PlayerInSightRange()
    {
        Vector3 playerPosition = playerTransform.position;

        return Vector2.Distance(transform.position, playerPosition) <= sightRange;
    }

    private bool PlayerInAttackRange()
    {
        Vector3 playerPosition = playerTransform.position;

        return Vector2.Distance(transform.position, playerPosition) <= attackRange;
    }

    private void Roam()
    {
        if (Vector2.Distance(transform.position, roamPosition) < 0.2f)
        {
            roamPosition = GetRoamingPosition();
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, roamPosition, roamSpeed * Time.deltaTime);
            animator.SetBool("Walking", true);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * roamRadius;
    }

    private void SeekPlayer()
    {
        Vector3 playerPosition = playerTransform.position;

        if (playerPosition != null && !Spawning)
        {
            Vector2 direction = playerPosition - transform.position;
            direction.Normalize();

            transform.Translate(direction * roamSpeed * Time.deltaTime);
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

    private void MaintainDistance()
    {
        Vector3 playerPosition = playerTransform.position;

        float distance = Vector2.Distance(transform.position, playerPosition);

        if (distance > maintainDistance)
        {
            SeekPlayer();
        }
        else if (distance < maintainDistance)
        {
            Vector2 direction = (transform.position - playerPosition).normalized;
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, moveSpeed * Time.deltaTime);
        }
    }

    void ChooseNewRoamDirection()
    {
        float angle = Random.Range(0f, 360f);
        roamDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction on collision
        roamDirection = -roamDirection;
    }

    void CheckPlayerDistance()
    {
        Vector3 playerPosition = playerTransform.position;

        if (Vector2.Distance(transform.position, playerPosition) <= detectionRange)
        {
            if (Time.time > nextFireTime)
            {
                FireProjectile();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FireProjectile()
    {
        Vector3 playerPosition = playerTransform.position;
        animator.SetTrigger("Shooting");
        animator.SetBool("Walking", false);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 directionToPlayer = (playerPosition - transform.position).normalized;
        Rigidbody2D rbProjectile = projectile.GetComponent<Rigidbody2D>();
        rbProjectile.velocity = directionToPlayer * roamSpeed;

        ParticleSystem particles = projectile.GetComponent<ParticleSystem>();
        if (particles != null)
        {
            particles.Play();
        }

        StartCoroutine(DestroyProjectileAfterTime(projectile, projectileLifetime));
    }

    IEnumerator DestroyProjectileAfterTime(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }

    //public void TakeDamage(int damage)
    //{
    //    Health -= damage;
    //    StartCoroutine(FlashSprite(hitColor, flashDuration));

    //    if (Health <= 0)
    //    {
    //        StartCoroutine(Die());
    //    }
    //}

    //private IEnumerator Die()
    //{
    //    currentState = State.Dead;
    //    bc2d.enabled = false;
    //    moveSpeed = 0;
    //    StartCoroutine(FlashSprite(hitColor, flashDuration));
    //    yield return new WaitForSeconds(flashDuration);
    //    animator.SetTrigger("Death");
    //    if (PlayerXp.Instance != null)
    //    {
    //        PlayerXp.Instance.EnemyKilled(xpGainOnDeath);
    //    }

    //    yield return new WaitForSeconds(deathAnimation.length);
    //    Destroy(gameObject);
    //}

    //private IEnumerator FlashSprite(Color color, float duration)
    //{
    //    spriteRenderer.color = color;
    //    yield return new WaitForSeconds(duration);
    //    spriteRenderer.color = Color.white;
    //}
}
