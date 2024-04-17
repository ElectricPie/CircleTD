using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] private float m_speed = 1.0f;

    private int m_damage = 1;
    private Unit m_target = null;

    public void SetupProjectile(int damage, Unit target)
    {
        m_damage = damage;
        m_target = target;
    }

    private void Update()
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
}
