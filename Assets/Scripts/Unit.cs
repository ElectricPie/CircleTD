using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Health { get; private set; }

    [SerializeField] [Min(0)] private int m_initialHealth = 20;

    protected void Awake()
    {
        Health = m_initialHealth;
    }
}
