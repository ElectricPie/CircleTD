using System.Collections;
using UnityEngine;

public class SetWaveSpawner : WaveSpawner
{
    [SerializeField] private WaveScriptableObject[] m_waves;

    protected override IEnumerator SpawnWave(int waveNumber)
    {
        WaveScriptableObject wave = m_waves[waveNumber];
        
        m_towerWaves.NewWave(wave);
        
        for (int i = 0; i < wave.UnitCount; i++)
        {
            m_towerWaves.AddUnitToLatestWave(wave.UnitPrefab);
            yield return new WaitForSeconds(wave.TimeSpawnGap);
        }

        Invoke(nameof(StartNextWave), m_maxTimeBetweenWaves);
    }
}
