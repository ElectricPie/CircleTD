using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject m_debugWaveOneUnit;
    [SerializeField] private GameObject m_debugWaveTwoUnit;

    // TODO: Consider changing to distance between units?
    [SerializeField] private float m_unitSpawnSpeed = 1.0f;
    [SerializeField] private Vector3 m_unitSpawnPoint;

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
    
    public void AddWave(GameObject waveUnit, int unitCount,float waveRotationsPerMinute)
    {
        StartCoroutine(SpawnUnitsCoroutine(waveUnit, unitCount, waveRotationsPerMinute));
    }

    // Start is called before the first frame update
    void Start()
    {
        m_waves = new List<Wave>();
        
        AddWave(m_debugWaveOneUnit, 4, 6.0f);
        AddWave(m_debugWaveTwoUnit, 2, 3.0f);
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

    private IEnumerator SpawnUnitsCoroutine(GameObject waveUnit, int unitCount, float waveRotationsPerMinute)
    {
        // Create the game object to rotate the units
        GameObject wave = new GameObject("Wave");
        wave.transform.parent = transform;
        wave.transform.localScale = Vector3.one;
        m_waves.Add(new Wave(wave.transform, waveRotationsPerMinute));
        
        // Create new units
        for (int i = 0; i < unitCount; i++)
        {
            Vector3 spawnPosition = transform.position + m_unitSpawnPoint;
            Instantiate(waveUnit, spawnPosition, Quaternion.identity, wave.transform);
            yield return new WaitForSeconds(m_unitSpawnSpeed);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + m_unitSpawnPoint, 0.5f);
    }
}
