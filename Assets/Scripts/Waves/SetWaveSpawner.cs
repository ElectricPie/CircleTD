using System.Collections;
using UnityEngine;

public class SetWaveSpawner : WaveSpawner
{
    [SerializeField] private WaveScriptableObject[] m_waves;

    protected override IEnumerator SpawnWave(int waveNumber)
    {
        WaveScriptableObject wave = m_waves[waveNumber];
        m_towerWaves.NewWave(wave.WaveRotationSpeed, wave.UnitCount);
        
        for (int i = 0; i < wave.UnitCount; i++)
        {
            m_towerWaves.SpawnUnitToLatestWave(wave.UnitPrefab);
            yield return new WaitForSeconds(wave.TimeSpawnGap);
        }
        
        WaveSpawningFinished();
    } 
}
