using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveScriptableObject[] m_waves;
    [Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawed")] 
    [SerializeField] private float m_maxTimeBetweenWaves = 30.0f;

    [SerializeField] private TowerWaves m_towerWaves;
    
    // Parameter is the new wave number
    public UnityEvent<int> OnWaveStartedEvent;

    private int m_waveCount = 0;
        
    private void Start()
    {
        if (m_towerWaves is null)
        {
            Debug.LogError($"Wave Manager on {name} is missing reference to a Tower Waves script", this);
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWave(WaveScriptableObject wave)
    {
        m_waveCount++;
        OnWaveStartedEvent.Invoke(m_waveCount);
        
        m_towerWaves.NewWave(wave.WaveRotationSpeed);
        
        for (int i = 0; i < wave.UnitCount; i++)
        {
            m_towerWaves.AddUnitToLatestWave(wave.UnitPrefab);
            yield return new WaitForSeconds(wave.TimeSpawnGap);
        }
    }

    private IEnumerator SpawnWaves()
    {
        // Spawn each wave
        foreach (WaveScriptableObject wave in m_waves)
        {
            yield return SpawnWave(wave);
            
            // TODO: Handle wave max spawning timer 
            yield return new WaitForSeconds(5.0f);
        }
    }
}
