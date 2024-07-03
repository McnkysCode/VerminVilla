using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private BossBar bossBar;

    public Bullet Attack;
    public TextMeshProUGUI ProceedTXT;

    public bool BossDefeated = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        bossBar = FindAnyObjectByType<BossBar>();
    }

    private void Start()
    {
        if (bossBar != null)
        {
            bossBar.SetMaxHealth(maxHealth);
        }
        Initialize();
    }

    public void Initialize()
    {
        GameObject proceedTextObject = GameObject.Find("ProceedTXT");
        if (proceedTextObject != null)
        {
            ProceedTXT = proceedTextObject.GetComponent<TextMeshProUGUI>();
        }

        if (ProceedTXT == null)
        {
            Debug.LogError("ProceedTXT not found in the scene!");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (bossBar != null)
        {
            bossBar.SetHealth(currentHealth);
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        BossDefeated = true;
        Debug.Log("Boss defeated!");
        if (ProceedTXT != null)
        {
            ProceedTXT.text = "You defeated the boss! You can go to the second floor now. Press E to proceed to the next floor.";
        }
        StartCoroutine(DestroyBoss());
    }

    private IEnumerator DestroyBoss()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}
