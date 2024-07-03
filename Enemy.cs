using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    public EnemyData Data;
    private Animator animator;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool Spawning = true;
    [SerializeField] private AnimationClip SpawnAnimation;
    public float maxVelocity = 5f; // Maximum allowed velocity

    public bool CanMove { get; set; } = true; // Add a flag to control movement

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    private void FixedUpdate()
    {
        CapVelocity();
    }

    private void CapVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void Update()
    {
        if (CanMove && player != null && !Spawning) // Check CanMove flag
        {
            Vector2 direction = player.position - transform.position;
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
