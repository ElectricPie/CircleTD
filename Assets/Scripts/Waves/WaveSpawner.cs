using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WaveSpawner : MonoBehaviour
{
    [SerializeField] protected TowerWaves m_towerWaves;
    [Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawned")] 
    [SerializeField] protected float m_maxTimeBetweenWaves = 30.0f;

    // Parameter is the new wave number
    public UnityEvent<int> OnWaveStartedEvent;
    public int CurrentWave { get; private set; } = 0;
    
    private List<IEnumerator> m_waveSpawningCoroutines;

    protected abstract IEnumerator SpawnWave(int waveNumber);
    
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

        IEnumerator newWaveCoroutine = SpawnWave(CurrentWave);
        m_waveSpawningCoroutines.Add(newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
        
        OnWaveStartedEvent.Invoke(CurrentWave);
    }
    
    protected void StartNextWave()
    {
        CurrentWave++;
        
        IEnumerator newWaveCoroutine = SpawnWave(CurrentWave);
        m_waveSpawningCoroutines.Add(newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
        
        OnWaveStartedEvent.Invoke(CurrentWave);
    }
}
