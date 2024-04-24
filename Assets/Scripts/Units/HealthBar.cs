using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    private Camera m_camera;

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = m_camera.transform.rotation;
    }
    
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
