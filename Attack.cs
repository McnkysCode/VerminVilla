using UnityEngine;
using System.Collections;
public class Attack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject firebalPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float bulletCooldown = 0.5f;
    public float bulletLifetime = 5f;
    private Animator ShootAnim;
    private float lastShootTime = 0f;
    private float bulletCooldownMultiplier = 1f;
    public int bulletDamageBonus = 0;
    public WeaponData weaponData;

    private bool canShootFireball = true; 
    private Transform target;
    private float shootDistance = 10f;
    public float fireInterval;
    private float fireballSpeed = 8f;

    private int numberOfFireballs = 1;
    void Start()
    {
        ShootAnim = GetComponent<Animator>();
        if (ShootAnim == null)
        {
            Debug.LogError("Animator component not found!");
        }
        StartCoroutine(ShootFireballsAtEnemies());
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > lastShootTime + bulletCooldown * bulletCooldownMultiplier)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }
    private void Shoot()
    {
        ShootAnim.SetTrigger("Shoot"); // Use a trigger instead of a boolean

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 shootDirection = (mousePosition - firePoint.position).normalized;

        // Calculate the angle using Atan2
        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.damage += bulletDamageBonus;
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;

        Destroy(bullet, bulletLifetime);
    }
    public void ShootFireball()
    {
        if (canShootFireball)
        {
            GameObject fireball = Instantiate(firebalPrefab, firePoint.position, Quaternion.identity);
            fireball.tag = "PlayerFireball";
            if (target != null)
            {
                Vector2 direction = (target.position - firePoint.position).normalized;
                Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
                rb.velocity = direction * fireballSpeed;
                Destroy(fireball, bulletLifetime);
            }
            canShootFireball = false;
            StartCoroutine(ResetFireballShoot());
        }
    }

    private IEnumerator ResetFireballShoot()
    {
        yield return new WaitForSeconds(fireInterval);
        canShootFireball = true;
    }

    private IEnumerator ShootFireballsAtEnemies()
    {
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null)
                {
                    Debug.LogWarning("Enemy is null");
                    continue;
                }

                if (Vector2.Distance(transform.position, enemy.transform.position) < shootDistance)
                {
                    target = enemy.transform;
                    if (target == null)
                    {
                        Debug.LogWarning("Target is null");
                        continue;
                    }

                    ShootFireball();
                    yield return new WaitForSeconds(fireInterval); 
                }
            }
            yield return new WaitForSeconds(0.1f); // Adjust as needed
        }
    }

    public void ApplyPowerUp(float cooldownMultiplier, int damageBonus, float duration)
    {
        bulletCooldownMultiplier = cooldownMultiplier;
        bulletDamageBonus = damageBonus;
        Invoke("RemovePowerUp", duration);
    }
    private void RemovePowerUp()
    {
        bulletCooldownMultiplier = 1f;
        bulletDamageBonus = 0;
    }
    public void UpgradeIncreaseFireballs(int increaseAmount)
    {
        numberOfFireballs += increaseAmount;
    }

}
