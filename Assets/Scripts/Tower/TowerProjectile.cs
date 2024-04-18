using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] private float m_speed = 1.0f;
    [Tooltip("Time after creation before projectile is destroyed")] [SerializeField] [Min(0)] private float m_destoryTime = 10.0f;

    private int m_damage = 1;
    private Unit m_target = null;

    public void SetupProjectile(int damage, Unit target)
    {
        m_damage = damage;
        m_target = target;
    }

    protected void Start()
    {
        Invoke(nameof(Timeout), m_destoryTime);
    }

    protected void Update()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if (m_target == null)
        {
            return;
        }

        float deltaDistance = m_speed * Time.deltaTime;
        Vector3 newPos = Vector3.MoveTowards(transform.position, m_target.transform.position, deltaDistance);
        transform.position = newPos;
        transform.LookAt(m_target.transform);
    }

    private void Timeout()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_target.gameObject)
        {
            m_target.Damage(m_damage);
            Destroy(this.gameObject);   
        }
    }
}
