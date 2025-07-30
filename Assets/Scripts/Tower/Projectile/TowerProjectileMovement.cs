using UnityEngine;

public class TowerProjectileMovement : MonoBehaviour
{
    public float Speed => m_speed;
    
    [Tooltip("In distance per second")] [SerializeField] private float m_speed = 20.0f;
    
    private Unit m_target = null;
    private Vector3 m_targetPos = Vector3.zero;
    
    public void SetTarget(Unit target, Vector3 targetPos)
    {
        m_target = target;
        m_targetPos = targetPos;
    }
    
    protected void Update()
    {
        MoveTowardsTarget();
    }
    
    private void MoveTowardsTarget()
    {
        transform.LookAt(m_targetPos);
        
        float moveDistance = m_speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
    }

}