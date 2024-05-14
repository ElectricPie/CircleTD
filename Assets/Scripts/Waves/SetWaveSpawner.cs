using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetWaveSpawner : WaveSpawner
{
    [SerializeField] private WaveScriptableObject[] m_waves;
    [Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawned")] 
    [SerializeField] private float m_maxTimeBetweenWaves = 30.0f;

    [SerializeField] private TowerWaves m_towerWaves;
    
    // Parameter is the new wave number
    public UnityEvent<int> OnWaveStartedEvent;

    public int CurrentWave { get; private set; } = 0;

    private List<IEnumerator> m_waveSpawningCoroutines;

    public void OnWaveKilled()
    {
        // Stop any waiting time
        CancelInvoke(nameof(StartNextWave));
        StartNextWave();
    }
    
    private void Start()
    {
        m_waveSpawningCoroutines = new List<IEnumerator>();
        
        if (m_towerWaves is null)
        {
            Debug.LogError($"Wave Manager on {name} is missing reference to a Tower Waves script", this);
            return;
        }

        IEnumerator newWaveCoroutine = SpawnWave(m_waves[CurrentWave]);
        m_waveSpawningCoroutines.Add(newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
    }

    private IEnumerator SpawnWave(WaveScriptableObject wave)
    {
        OnWaveStartedEvent.Invoke(CurrentWave);
        
        m_towerWaves.NewWave(wave);
        
        for (int i = 0; i < wave.UnitCount; i++)
        {
            m_towerWaves.AddUnitToLatestWave(wave.UnitPrefab);
            yield return new WaitForSeconds(wave.TimeSpawnGap);
        }

        Invoke(nameof(StartNextWave), m_maxTimeBetweenWaves);
    }

    private void StartNextWave()
    {
        CurrentWave++;
        
        IEnumerator newWaveCoroutine = SpawnWave(m_waves[CurrentWave]);
        m_waveSpawningCoroutines.Add(newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
    }
}
