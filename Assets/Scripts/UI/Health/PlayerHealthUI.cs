using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Health healthComponent;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (healthComponent == null)
            healthComponent = GetComponent<Health>();
    }

    private void OnEnable()
    {
        healthComponent.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        healthComponent.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        Debug.Log($"Ativado: {current}, {max}");
        slider.maxValue = max;
        slider.value = current;
    }
}
