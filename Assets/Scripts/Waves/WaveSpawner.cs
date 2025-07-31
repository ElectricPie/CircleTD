using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WaveSpawner : MonoBehaviour
{
    [SerializeField] protected TowerWaves m_towerWaves;
    [Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawned")] 
    [SerializeField] private float m_maxTimeBetweenWaves = 30.0f;

    // Parameter is the new wave number
    public UnityEvent<int> OnWaveStartedEvent;
    public int CurrentWave { get; private set; } = 0;
    
    // private readonly List<IEnumerator> m_waveSpawningCoroutines = new List<IEnumerator>();
    private readonly Dictionary<int, IEnumerator> m_waveSpawningCoroutines = new Dictionary<int, IEnumerator>();

    protected abstract IEnumerator SpawnWave(int waveNumber);
    
    protected void WaveFinished(Wave completedWave)
    {
        // Stop any waiting time
        CancelInvoke(nameof(StartNextWave));
        StartNextWave();
        m_waveSpawningCoroutines.Remove(completedWave.WaveNumber);
    }

    private void Start()
    {
        if (m_towerWaves == null)
            throw new Exception($"Wave Manager on {name} is missing reference to a Tower Waves script");

        StartWave();
    }

    protected void WaveSpawningFinished(int waveNumber)
    {
        m_waveSpawningCoroutines.Remove(waveNumber);
        Invoke(nameof(StartNextWave), m_maxTimeBetweenWaves);
    }
    
    protected void StartNextWave()
    {
        CurrentWave++;
        
        StartWave();
    }

    private void StartWave()
    {
        IEnumerator newWaveCoroutine = SpawnWave(CurrentWave);
        m_waveSpawningCoroutines.Add(CurrentWave, newWaveCoroutine);
        StartCoroutine(newWaveCoroutine);
        
        OnWaveStartedEvent.Invoke(CurrentWave);
    }
}
