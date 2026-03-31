using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHpBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Entity_Health playerHealth;

    private void Start()
    {
        playerHealth.OnHealthChanged += UpdateUI;
        UpdateUI(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    public void Initialize(Entity_Health health)
    {
        health.OnHealthChanged += UpdateUI;
        UpdateUI(health.GetCurrentHealth(), health.GetMaxHealth());
    }

    private void UpdateUI(float current, float max)
    {
        slider.value = current / max;

        if (hpText != null)
            hpText.text = $"{current} / {max}";
    }
}
