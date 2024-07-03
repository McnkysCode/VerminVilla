using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisenThrow : MonoBehaviour
{
    private GameObject leakyFlaskPrefab;
    private bool canThrowLeakyFlask = true;
    public Transform throwPoint;
    private Transform target;
    private float shootDistance = 5f;
    public float poisenInterval;
    public float poisenSpeed = 3f;
    public float poisenLifetime = 4f;

    private void Start()
    {
        StartCoroutine(ShootLeakyFlaskAtEnemies());
    }
    public void ThrowLeakyFlask()
    {
        if (canThrowLeakyFlask)
        {
            GameObject poisenFlask = Instantiate(leakyFlaskPrefab, throwPoint.position, Quaternion.identity);
            poisenFlask.tag = "PlayerFireball";
            if (target != null)
            {
                Vector2 direction = (target.position - throwPoint.position).normalized;
                Rigidbody2D rb = poisenFlask.GetComponent<Rigidbody2D>();
                rb.velocity = direction * poisenSpeed;
                Destroy(poisenFlask, poisenLifetime);
                LeakyFlaskDelay();
            }
            canThrowLeakyFlask = false;
            StartCoroutine(ResetLeakyflask());
        }
    }
    public IEnumerator LeakyFlaskDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator ResetLeakyflask()
    {
        yield return new WaitForSeconds(poisenInterval);
        canThrowLeakyFlask = true;
    }
    private IEnumerator ShootLeakyFlaskAtEnemies()
    {
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                if (Vector2.Distance(transform.position, enemy.transform.position) < shootDistance)
                {
                    target = enemy.transform;
                    ThrowLeakyFlask();
                    break;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
