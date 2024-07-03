using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2StairsIteract : MonoBehaviour
{
    private BossHealth bossHealth;

    private void Update()
    {
        if (bossHealth == null)
        {
            GameObject boss = GameObject.FindWithTag("Boss");
            if (boss != null)
            {
                bossHealth = boss.GetComponent<BossHealth>();
                Debug.Log("BossHealth component found and assigned.");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (bossHealth != null && bossHealth.BossDefeated)
        {
            Debug.Log("Boss is defeated.");
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player is in the trigger area.");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("E key pressed. Loading next level...");
                    LoadNextLevel();
                }
            }
            else
            {
                Debug.Log("Other object in trigger is not Player: " + other.tag);
            }
        }
        else
        {
            Debug.Log("Boss is not defeated or bossHealth is null.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called with object: " + other.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D called with object: " + other.gameObject.name);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level2");
    }
}
