using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform m_debugWaveOne;
    [SerializeField] private Transform m_debugWaveTwo;

    private List<Wave> m_waves;

    struct Wave
    {
        public Wave(Transform waveTransform, float rotationsPerMinute)
        {
            WaveTransform = waveTransform;
            RotationsPerMinute = rotationsPerMinute;
        }
        
        public Transform WaveTransform;
        public float RotationsPerMinute;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        m_waves = new List<Wave>();
        
        m_waves.Add(new Wave(m_debugWaveOne, 3.0f));
        m_waves.Add(new Wave(m_debugWaveTwo, 6.0f));
    }

    // Update is called once per frame
    void Update()
    {
        RotateWaves();
    }

    private void RotateWaves()
    {
        // Rotate each wave based on each ones RotationsPerMinute
        foreach (Wave wave in m_waves)
        {
            // Convert RPM to angle by / 60 to get seconds and * by 360 to convert to angle
            float rotationAngle = wave.RotationsPerMinute / 60 * 360 * Time.deltaTime;
            wave.WaveTransform.Rotate(Vector3.up, rotationAngle);
        }
    }
}
