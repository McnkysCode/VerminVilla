using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXp : MonoBehaviour
{
    [SerializeField] private Slider sliderXp;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private UpgradeCards upgradecards;
    public static PlayerXp Instance;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] public AnimationClip levelAnimation;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI xpText;

    public int currentXp = 0;
    public int maxXp = 100;
    public int Level = 1;

    private float xpMultiplier = 1f; 

    void Awake()
    {
        sliderXp.value = currentXp;
        upgradecards = FindObjectOfType<UpgradeCards>();
        Instance = this;
        countdownText.enabled = false;
        xpText.enabled = false;
    }
    public void EnemyKilled(int xpGainedOnDeath)
    {
        int xpToAdd = Mathf.RoundToInt(xpGainedOnDeath * xpMultiplier);
        currentXp += xpToAdd;
        Debug.Log($"Enemy Killed: XP Gained = {xpToAdd}, Multiplier = {xpMultiplier}");
        currentXp = Mathf.Min(currentXp, maxXp);

        UpdateUI();
        if (sliderXp.value >= maxXp)
        {
            LevelPlayer();
        }
    }

    private void UpdateUI()
    {
        if (sliderXp != null)
        {
            sliderXp.value = (float)currentXp / maxXp;
        }
        if (sliderXp != null && sliderXp.value == maxXp)
        {
            currentXp = 0;
            sliderXp.value = 0;
        }
    }

    public void LevelPlayer()
    {
        playerAnimator.SetTrigger("LevelUp");
        if (sliderXp.value == 1.0f)
        {
            currentXp = 0;
            maxXp += 50;
            Level += 1;
            UpdateUI();
            UpdateLevelText();
            upgradecards.HandleLevelUp();
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = Level.ToString();
        }
    }

    public void ApplyXpBoost(float multiplier, float duration)
    {
        xpMultiplier = multiplier;
        StartCoroutine(Countdown(duration));
        Invoke(nameof(ResetXpBoost), duration);
    }

    private IEnumerator Countdown(float duration)
    {
        float timeLeft = duration;
        while (timeLeft > 0)
        {
            countdownText.enabled = true;
            xpText.enabled = true;
            countdownText.text = Mathf.Ceil(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
        }
        //countdownText.text = "x2 Boost 0";
        
    }

    private void ResetXpBoost()
    {
        xpMultiplier = 1f;
        Debug.Log("XP Boost Reset to normal");
        countdownText.text ="";
        countdownText.enabled = false;
        xpText.enabled = false;
    }
}



