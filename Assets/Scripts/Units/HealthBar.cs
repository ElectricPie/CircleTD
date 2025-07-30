using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private UnitHealth m_unitHealth;
    [SerializeField] private Slider m_slider;
    
    private Camera m_camera;

    private void Awake()
    {
        if (m_unitHealth == null)
        {
            throw new System.Exception($"HealthBar script on {name} is missing reference to UnitHealth component.");
        }
        
        if (m_slider == null)
        {
            throw new System.Exception($"HealthBar script on {name} is missing reference to Slider component.");
        }

        m_unitHealth.OnUnitMaxHealthChangedEvent += OnMaxHealthChanged;
        m_unitHealth.OnUnitCurrentHealthChangedEvent += OnCurrentHealthChanged;
        
        m_slider.maxValue = m_unitHealth.MaxHealth;
        m_slider.value = m_unitHealth.CurrentHealth;
        
        m_camera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = m_camera.transform.rotation;
    }

    private void OnCurrentHealthChanged(float currentHealth)
    {
        m_slider.value = currentHealth;
    }

    private void OnMaxHealthChanged(float newMaxHealth)
    {
        m_slider.maxValue = newMaxHealth;
    }
}
