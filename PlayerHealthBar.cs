using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerHealth health;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (health == null)
        {
            health = FindObjectOfType<PlayerHealth>();
        }

        if (health != null)
        {
            UpdateHealthbarMax(health.maxHealth);
        }
    }

    public void UpdateHealthbarMax(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void UpdateHealthBar(int currentHealth)
    {
        slider.value = currentHealth;
    }
}