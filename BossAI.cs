using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private BossHealth bossHealth;

    private void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        bossHealth = GetComponent<BossHealth>();
    }
}