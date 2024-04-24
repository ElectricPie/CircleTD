using System.Collections.Generic;
using UnityEngine;

public class TowerWaves : MonoBehaviour
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
            Units = new List<Unit>();
        }
        
        public Transform WaveTransform;
        public float RotationsPerMinute;
        public List<Unit> Units;
    }

    public void NewWave(float waveRotationsPerMinute)
    {
        // TODO: Create wave finished callback
        
        // Create the game object to rotate the units
        GameObject waveGameObject = new GameObject("Wave");
        waveGameObject.transform.parent = transform;
        waveGameObject.transform.localScale = Vector3.one;

        Wave wave = new Wave(waveGameObject.transform, waveRotationsPerMinute);
        m_waves.Add(wave);
    }

    public void AddUnitToLatestWave(GameObject unitPrefab)
    {
        Wave latestWave = m_waves[m_waves.Count - 1];
        
        Vector3 spawnPosition = transform.position + m_unitSpawnPoint;
        Unit newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, latestWave.WaveTransform.transform).GetComponent<Unit>();
        if (newUnit == null)
        {
            // Remove the unit if it doesn't have the unit component
            Debug.Log($"{this.name} is attempting to spawn unit that is missing the unit component", this);
            Destroy(newUnit.gameObject);
        }
        else
        {
            newUnit.OnUnitKilledEvent += OnUnitKilled;
            latestWave.Units.Add(newUnit);
        }
    }
    
    /// <summary>
    /// Gets the earliest spawned <c>Unit</c> in the earliest wave
    /// </summary>
    /// <returns>The oldest spawned <c>Unit</c> if there are waves other wise null</returns>
    public Unit GetOldestUnit()
    {
        if (m_waves == null)
        {
            return null;
        }
        
        if (m_waves.Count == 0)
        {
            return null;
        }
        
        Wave oldestWave = m_waves[0];
        if (oldestWave.Units.Count == 0)
        {
            return null;
        }
        
        return oldestWave.Units[0];
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_waves = new List<Wave>();
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

    private void OnUnitKilled(Unit killedUnit)
    {
        foreach (Wave wave in m_waves)
        {
            // Remove the killed unit
            if (wave.Units.Remove(killedUnit))
            {
                // Remove the wave once all units have been killed
                if (wave.Units.Count == 0)
                {
                    m_waves.RemoveAt(0);
                    Destroy(wave.WaveTransform.gameObject);
                }
                return;
            }
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + m_unitSpawnPoint, 0.5f);
    }
}
