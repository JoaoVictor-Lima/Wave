using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    // Pode ser setado no Inspector (para inimigos via EnemyHealthUI)
    // ou via Initialize (para o player local via HUDController)
    [SerializeField] private Health healthComponent;
    [SerializeField] private Slider slider;

    public void Initialize(Health health)
    {
        if (healthComponent != null)
            healthComponent.OnHealthChanged -= UpdateHealthBar;

        healthComponent = health;
        healthComponent.OnHealthChanged += UpdateHealthBar;
    }

    private void OnEnable()
    {
        if (healthComponent != null)
            healthComponent.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        if (healthComponent != null)
            healthComponent.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
