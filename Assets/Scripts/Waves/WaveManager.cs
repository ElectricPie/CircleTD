using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveScriptableObject[] m_waves;
    [Tooltip("The time in seconds before the next wave spawns if the current one is not complete. Starts after the last unit is spawed")] 
    [SerializeField] private float m_maxTimeBetweenWaves = 30.0f;

    [SerializeField] private TowerWaves m_towerWaves;

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
            
            // TEMP
            yield return new WaitForSeconds(5.0f);
        }
        
        yield return null;
    }
}
