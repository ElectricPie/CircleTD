using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName="New Wave", menuName="Waves/New Wave")]
public class WaveScriptableObject : ScriptableObject
{
    public GameObject UnitPrefab;
    [Min(1)] public int UnitCount = 4;
    [Tooltip("The time in seconds between each unit spawning in a wave")]
    [Min(0.1f)] public float TimeSpawnGap = 1.0f;
    [Tooltip("How fast the wave will rotate around the tower in Rotations Per Minute")]
    [Min(0.1f)] public float WaveRotationSpeed = 3.0f;
}
