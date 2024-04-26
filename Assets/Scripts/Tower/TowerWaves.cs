using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerWaves : MonoBehaviour
{
    [SerializeField] private Vector3 m_unitSpawnPoint;
    [Tooltip("If set to a value will move a new spawned unit left/right the amount of the value")]
    [SerializeField] private float m_unitSpawnPointVairation = 0.0f;

    public UnityEvent<Unit> OnUnitSpanwedEvent;
    public UnityEvent<Unit> OnUnitKilledEvent;

    public UnityEvent OnWaveKilledEvent;

    private List<Wave> m_waves;
    private int m_unitCount;

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
        if (unitPrefab is null)
        {
            Debug.Log($"{this.name} is missing the unit prefab", this);
            return;
        }
        
        if (unitPrefab.GetComponent<Unit>() is null)
        {
            Debug.Log($"{this.name} is attempting to spawn unit that is missing the unit component", this);
            return;
        }
        
        Wave latestWave = m_waves[m_waves.Count - 1];
        
        Vector3 spawnPosition = transform.position + m_unitSpawnPoint;
        spawnPosition.x += Random.Range(-m_unitSpawnPointVairation, m_unitSpawnPointVairation);
        Unit newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity, latestWave.WaveTransform.transform).GetComponent<Unit>();
        
        newUnit.OnUnitKilledEvent += OnUnitKilled;
        latestWave.Units.Add(newUnit);

        m_unitCount++;
        OnUnitSpanwedEvent.Invoke(newUnit);
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
            if (wave.Units.Contains(killedUnit))
            {
                OnUnitKilledEvent.Invoke(killedUnit);
                wave.Units.Remove(killedUnit);
                
                // Remove the wave once all units have been killed
                if (wave.Units.Count == 0)
                {
                    m_waves.RemoveAt(0);
                    Destroy(wave.WaveTransform.gameObject);
                    OnWaveKilledEvent.Invoke();
                }

                m_unitCount--;
                return;
            }
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        // Draw spawn point
        Vector3 spawnPoint = transform.position + m_unitSpawnPoint;
        Gizmos.DrawSphere(spawnPoint, 0.5f);
        
        // Draw spawn variation
        Vector3 leftOfSpawn = spawnPoint;
        leftOfSpawn.x -= m_unitSpawnPointVairation;
        Vector3 rightOfSpawn = spawnPoint;
        rightOfSpawn.x += m_unitSpawnPointVairation;
        Gizmos.DrawLine(leftOfSpawn, rightOfSpawn);
    }
}