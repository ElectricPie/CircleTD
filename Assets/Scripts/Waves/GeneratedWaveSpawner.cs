using System.Collections;
using UnityEngine;

public class GeneratedWaveSpawner : WaveSpawner
{
    // Time is the wave and the value is the modifier for unit stats
    [SerializeField] private AnimationCurve m_waves;
    [SerializeField] private int m_unitWaveCount = 40;
    [SerializeField] private float m_waveRotationalSpeed = 10.0f;
    [SerializeField] private float m_waveUnitTimeGap = 0.5f;
    
    [SerializeField] private Unit m_unitBase;

    [SerializeField] private float m_unitStartingHealth = 4.0f;
    private float m_unitStartingMoneyWorth = 1.0f;

    protected override IEnumerator SpawnWave(int waveNumber)
    {
        Wave newWave = m_towerWaves.NewWave(m_waveRotationalSpeed, m_unitWaveCount, waveNumber);
        newWave.OnAllUnitsKilled += WaveFinished;
        
        float unitHealth = m_unitStartingHealth * m_waves.Evaluate(waveNumber);
        float unitWorth = m_unitStartingMoneyWorth * m_waves.Evaluate(waveNumber);

        for (int i = 0; i < m_unitWaveCount; i++)
        {
            m_towerWaves.SpawnUnitToLatestWave(m_unitBase, true, unitHealth, unitWorth);
            yield return new WaitForSeconds(m_waveUnitTimeGap);
        }
        
        WaveSpawningFinished(waveNumber);
    }
}
