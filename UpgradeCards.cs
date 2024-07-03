using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCards : MonoBehaviour
{
    [SerializeField] private GameObject[] cards;
    [SerializeField] private Transform[] cardPositions;
    [SerializeField] private GameObject cardPanel;
    [SerializeField] private WeaponData weapondata;
    [SerializeField] private Movement movementPlayer;
    [SerializeField] private TextMeshProUGUI levelupNoification;
    [SerializeField] private GameObject levelupObject;

    private PlayerHealth playerhealth;
    private PlayerHealthBar playerhealthbar;
    private Animator anim;
    private EnemyWave enemywave;
    private PlayerXp playerXp;
    private Enemy enemy;
    private Attack attack;
    private Rigidbody2D currentlyFrozenEnemy;
    private EnemyData enemydata;
    private Movement movement;
    private BroomPushback broomPushback;
    private FireballProj fireballproj;
    private ButtonManager buttons;
    
    private bool levelupReady;

    private Dictionary<Rigidbody2D, Coroutine> frozenEnemies = new Dictionary<Rigidbody2D, Coroutine>();
    void Awake()
    {
        levelupReady = false;
        if (cardPanel != null)
        {
            levelupObject.SetActive(false);
            cardPanel.SetActive(false);
        }
        Time.timeScale = 1f;
        playerXp = FindFirstObjectByType<PlayerXp>();
        playerhealth = FindFirstObjectByType<PlayerHealth>();
        playerhealthbar = FindFirstObjectByType<PlayerHealthBar>();
        anim = FindFirstObjectByType<Animator>();
        attack = FindObjectOfType<Attack>();
        enemydata = FindObjectOfType<EnemyData>();
        movement = FindObjectOfType<Movement>();
        broomPushback = FindObjectOfType<BroomPushback>();
        fireballproj = GetComponent<FireballProj>();
        buttons = FindAnyObjectByType<ButtonManager>();
    }
    private void Update()
    {
        if (playerXp.currentXp >= playerXp.maxXp)
        {
            levelupReady = true;
            levelupNoification.enabled = true;
            CardsAnimation();
            playerXp.LevelPlayer();
            CheckingforLevelUp();
        }
    }
    private void CheckingforLevelUp()
    {
        if (levelupReady)
        {
            Debug.Log("Checkforlevelup");
            RandomUpgradeCards();
        }
    }
    public void RandomUpgradeCards()
    {
        foreach (GameObject card in cards)
        {
            card.SetActive(false);
        }
        List<GameObject> shuffledCards = new List<GameObject>(cards);
        Shuffle(shuffledCards);                                                               

        for (int i = 0; i < 3; i++)
        {
            GameObject selectedCard = shuffledCards[i];
            selectedCard.SetActive(true);
            selectedCard.transform.position = cardPositions[i].position;

            HandleUpgrade(selectedCard);
        }
    }
    private void Shuffle(List<GameObject> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    private void HandleUpgrade(GameObject selectedCard)
    {
        Debug.Log("HandleUpgrade method called for card: " + selectedCard);
        switch (selectedCard.name)
        {
            case "Upgrade1":
                Upgrade1_BroomItem();
                break;
            case "Upgrade2":
                Upgrade2_HauntedLantern();
                break;
            case "Upgrade4":
                Upgrade3_GentelmanBowtie();
                break;
            case "Upgrade5":
                Upgrade4_SwanFeather();
                break;
            case "Upgrade6":
                Upgrade5_GoldenStopWatch();
                break;
            case "Upgrade9":
                Upgrade6_PiperBoots();
                break;
            case "Upgrade10":
                Upgrade7_healthPotion();
                break;
            default:
                Debug.Log("Unknown card name: " + selectedCard.name);
                break;
        }
    }

    public void Upgrade1_BroomItem()
    {
        if (broomPushback != null)
        {
            float newInterval = Mathf.Max(broomPushback.broomInterval - 1f, 2f); // Decrease interval by 1 second
            broomPushback.UpgradeBroomInterval(newInterval); // Apply the upgraded interval
            Debug.Log("Pressed 1");
        }
        UpgradeComplete(); 
    }
    public void Upgrade2_HauntedLantern()
    {
        if (weapondata != null && cardPanel != null)
        {
            attack.fireInterval = Mathf.Max(attack.fireInterval - 1f, 0.5f);
            attack.UpgradeIncreaseFireballs(1);
            Debug.Log("Pressed 2");
            UpgradeComplete();
        }
    }
    public void Upgrade3_GentelmanBowtie ()
    {
        if (playerhealth != null & cardPanel != null)
        {
            movementPlayer.moveSpeed += 1f;
            Debug.Log("Pressed 4");
            UpgradeComplete();
        }
    }
    public void Upgrade4_SwanFeather()
    {
        if (cardPanel != null && attack != null)
        {
            attack.bulletSpeed += 2;
            Debug.Log("Pressed 5");
            UpgradeComplete();
        }
    }
    public void Upgrade5_GoldenStopWatch()
    {
        if (cardPanel != null)
        {
            PlayerXp playerXp = PlayerXp.Instance;
            if (playerXp != null)
            {
                playerXp.ApplyXpBoost(2f, 10f); 
                UpgradeComplete();
            }
        }
    }
    public void Upgrade6_PiperBoots()
    {
        if (movement != null && cardPanel != null)
        {
            movement.dashCooldown -= 1f;
            UpgradeComplete();
        }
    }
    public void Upgrade7_healthPotion()
    {
        if (playerhealth != null)
        {
            HealthCalculation();
            UpgradeComplete();
        }
    }
    private void HealthCalculation()
    {
        if (playerhealth.currentHealth >= playerhealth.maxHealth)
        {
            playerhealth.currentHealth += 0;
            playerhealthbar.slider.value += 0;
          
        }
        if (playerhealth.currentHealth < playerhealth.maxHealth)
        {
            int healthToAdd = Mathf.Min(30, playerhealth.maxHealth - playerhealth.currentHealth);
            playerhealth.currentHealth += healthToAdd;
            playerhealthbar.slider.value += healthToAdd;
            
        }
        if (playerhealth.currentHealth <= 70)
        {
            int bonusHealth = Mathf.Min(30, playerhealth.maxHealth - playerhealth.currentHealth);
            playerhealth.currentHealth += bonusHealth;
            playerhealthbar.slider.value += bonusHealth;
        }
    }
    public void HandleLevelUp()
    {
        if (levelupReady)
        {
            levelupObject.SetActive(true);
            StartCoroutine(DelayCards());
        }
    }
    IEnumerator DelayCards()
    {
        yield return new WaitForSeconds(0.53f);
        cardPanel.SetActive(true);
        buttons.SetInteractibleCards(false);
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(0.4f);
        movement.enabled = false;
        buttons.SetInteractibleCards(true);
        Time.timeScale = 0f;
    }
    public void UpgradeComplete()
    {
        if (cardPanel != null)
        {
            StartCoroutine(SelectedCardsDelay());
            levelupObject.SetActive(false);
            Debug.Log("upgrade complete");
            Time.timeScale = 1f;
            movement.enabled = true;
            cardPanel.SetActive(false);
            levelupReady = false;
        }
    }
    private void CardsAnimation()
    {
        if (anim != null)
        {
            anim.SetBool("Appear", true);
            anim.SetBool("Cardanim", true);
            anim.SetBool("AppearBackground", true);
            anim.SetBool("cardAppear", true);
        }
    }
    IEnumerator SelectedCardsDelay()
    {
        yield return new WaitForSeconds(2f);
    }
}
