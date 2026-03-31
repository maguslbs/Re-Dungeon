using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStaminaUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private Entity_Stamina playerStamina;

    private void Start()
    {
        playerStamina.OnStaminaChanged += UpdateUI;

        // update awal
        UpdateUI(playerStamina.GetCurrentStamina(), playerStamina.GetMaxStamina());
    }

    public void Initialize(Entity_Stamina stamina)
    {
        stamina.OnStaminaChanged += UpdateUI;
        UpdateUI(stamina.GetCurrentStamina(), stamina.GetMaxStamina());
    }

    private void UpdateUI(float current, float max)
    {
        // Update bar
        slider.value = current / max;

        // Update text (format seperti di gambar kamu)
        if (staminaText != null)
            staminaText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
    }

    private void OnDestroy()
    {
        if (playerStamina != null)
            playerStamina.OnStaminaChanged -= UpdateUI;
    }
}