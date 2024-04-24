using TMPro;
using UnityEngine;

public class WaveTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text m_waveNumberText;
    
    public void OnWaveStarted(int waveNumber)
    {
        if (m_waveNumberText is null)
        {
            Debug.LogError($"Wave Tracker on {name} is missing reference to WaveNumberText", this);
            return;
        }

        m_waveNumberText.text = waveNumber.ToString("000");
    }
}
