using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyWave
{
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    public bool isBossWave; // Add this to distinguish boss waves
}

public class WaveSystem : MonoBehaviour
{
    public EnemyWave[] waves;
    public Transform[] spawnPoints;
    public Transform powerUpSpawnPoint;
    public GameObject powerUpPrefab;
    public GameObject particleEffectPrefab;
    public List<Transform> particleEffectWaypoints;
    public float timeBetweenWaves = 5f;
    public float powerUpSpawnInterval = 3 * 3;
    public float powerUpLifetime = 10f;
    public float powerUpCooldown = 5f;
    public bool handlesBoss; // Add this flag to indicate if this instance handles the boss

    private float countdownTimer = 0f;
    private float powerUpCooldownTimer = 0f;
    private int currentWaveIndex = 0;
    private int wavesSpawned = 0;
    private bool waveSystemStarted = false;
    private bool spawningWave = false;
    private List<GameObject> activeParticleEffects;

    private void Start()
    {
        activeParticleEffects = new List<GameObject>();
    }

    private void Update()
    {
        if (waveSystemStarted && !spawningWave)
        {
            if (currentWaveIndex < waves.Length)
            {
                if (AllEnemiesKilled())
                {
                    SpawnNextWave();
                }
            }
            else
            {
                waveSystemStarted = false;
                foreach (var effect in activeParticleEffects)
                {
                    Destroy(effect);
                }
                activeParticleEffects.Clear();
            }

            countdownTimer -= Time.deltaTime;
            powerUpCooldownTimer -= Time.deltaTime;
        }
    }

    private void SpawnNextWave()
    {
        if (countdownTimer <= 0f || AllEnemiesKilled())
        {
            EnemyWave wave = waves[currentWaveIndex];
            if (wave.isBossWave && handlesBoss)
            {
                SpawnBossWave(wave);
            }
            else
            {
                SpawnWave(wave);
            }

            countdownTimer = timeBetweenWaves;
            currentWaveIndex++;
            wavesSpawned++;

            if (wavesSpawned % 3 == 0)
            {
                SpawnPowerUp();
                powerUpCooldownTimer = powerUpCooldown;
            }
        }
    }

    private bool AllEnemiesKilled()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;
    }

    public void StartWaveSystem()
    {
        waveSystemStarted = true;
        if (particleEffectPrefab != null && particleEffectWaypoints != null)
        {
            foreach (var waypoint in particleEffectWaypoints)
            {
                GameObject effect = Instantiate(particleEffectPrefab, waypoint.position, waypoint.rotation);
                activeParticleEffects.Add(effect);
            }
        }
    }

    private void SpawnWave(EnemyWave wave)
    {
        spawningWave = true;
        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnPointIndex];
            Instantiate(wave.enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
        spawningWave = false;
    }

    private void SpawnBossWave(EnemyWave wave)
    {
        spawningWave = true;
        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnPointIndex];
            GameObject bossInstance = Instantiate(wave.enemyPrefab, spawnPoint.position, Quaternion.identity);
            BossHealth bossHealth = bossInstance.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.Initialize();
            }
        }
        spawningWave = false;
    }

    private void SpawnPowerUp()
    {
        if (powerUpSpawnPoint != null)
        {
            GameObject powerUp = Instantiate(powerUpPrefab, powerUpSpawnPoint.position, Quaternion.identity);
            Destroy(powerUp, powerUpLifetime);
        }
        else
        {
            Debug.LogError("Power-up spawn point is not assigned!");
        }
    }

    public bool CanSpawnWave()
    {
        return currentWaveIndex < waves.Length && powerUpCooldownTimer <= 0;
    }
}
