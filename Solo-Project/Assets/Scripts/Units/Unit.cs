using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform transform;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float maxDistanceDelta = 0.5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float moveSpeed = 9f;
    
    private Vector3 targetPosition;
    private bool shouldMove;
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

    public void ToggleKinematics(bool toggle)
    {
        rigidbody.isKinematic = toggle;
    }

    public void ResetVelocity()
    {
        rigidbody.linearVelocity = Vector3.zero;
    }

    public void MoveTowardsPoint()
    {
        if (!shouldMove)
        {
            return;
        }
        
        float activeSpeed = maxSpeed;

        Vector3 steerDirection = (targetPosition - transform.position).normalized;

        float steerMagnitude = Vector3.Distance(transform.position, targetPosition);

        if (steerMagnitude > maxDistanceDelta)
        {
            rigidbody.AddForce(steerDirection * (activeSpeed * moveSpeed), ForceMode.Acceleration);
        }
        else
        {
            activeSpeed = 0.0f;
            ResetVelocity();
            ToggleKinematics(true);
            shouldMove = false;
        }
        
        rigidbody.linearVelocity = Vector3.ClampMagnitude(rigidbody.linearVelocity, maxSpeed);
    }

    public void SetDestination(Vector3 point)
    {
        targetPosition = point;
    }

    public void ToggleMove(bool toggle)
    {
        shouldMove = toggle;
    }
    
}
