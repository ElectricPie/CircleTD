using UnityEngine;

[CreateAssetMenu(fileName="New Wave", menuName="Waves/New Wave")]
public class WaveScriptableObject : ScriptableObject
{
    public GameObject m_waveUnitPrefab;
    [Min(1)] public int m_maveUnitCount = 4;
}
