using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_slider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        if (m_slider is null)
        {
            Debug.LogError($"{name} is missing reference to the slider in the HealthBar script", this);
            return;
        }
        
        m_slider.value = currentValue / maxValue;
    }
}
