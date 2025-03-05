using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform transform;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float maxDistanceDelta = 0.5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float moveSpeed = 9f;
    
    private float radius = 0.5f;

    public void SteerTowardsLeader(Transform leader)
    {

        float activeSpeed = maxSpeed;

        Vector3 steerDirection = (leader.transform.position - transform.position).normalized;

        float steerMagnitude = Vector3.Distance(transform.position, leader.transform.position);

        if (steerMagnitude > maxDistanceDelta)
        {
            rigidbody.AddForce(steerDirection * (activeSpeed * moveSpeed), ForceMode.Acceleration);
        }
        else
        {
            activeSpeed = 0.0f;
            //rigidbody.AddForce(0, 0, 0, ForceMode.Force  );
            rigidbody.linearVelocity = Vector3.zero;
        }
        
        rigidbody.linearVelocity = Vector3.ClampMagnitude(rigidbody.linearVelocity, maxSpeed);
    }

    public void MoveWithLeader(Vector3 leaderAppliedForce)
    {
        rigidbody.AddForce(leaderAppliedForce, ForceMode.Force);
    }

    public void SetRBDrag(float drag)
    {
        rigidbody.linearDamping = drag;
    }

    public float GetRadius()
    {
        return radius;
    }
}
