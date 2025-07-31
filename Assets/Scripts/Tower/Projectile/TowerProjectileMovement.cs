using UnityEngine;

public class TowerProjectileMovement : MonoBehaviour
{
    public float Speed => m_speed;
    public Vector3 TargetPos = Vector3.zero;
    
    [Tooltip("In distance per second")] [SerializeField] private float m_speed = 20.0f;
    
    protected void Update()
    {
        MoveTowardsTarget();
    }
    
    private void MoveTowardsTarget()
    {
        transform.LookAt(TargetPos);
        
        float moveDistance = m_speed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
    }
}