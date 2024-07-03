using System.Collections;
using UnityEngine;

public class ChandelierBehavior : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float detectionRange = 5.0f;
    public GameObject projectilePrefab;
    public Transform player;
    private Vector2 roamDirection;
    public float fireRate = 2.0f;
    public float projectileLifetime = 3.0f;

    private Rigidbody2D rb;
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextFireTime = Time.time;
        ChooseNewRoamDirection();
        StartCoroutine(DestroyAfterTime(20f));
    }

    void Update()
    {
        Roam();
        CheckPlayerDistance();
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
        // Reverse direction on collision
        roamDirection = -roamDirection;
    }

    void CheckPlayerDistance()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRange)
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
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

    // Coroutine to destroy the GameObject after a set time
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
