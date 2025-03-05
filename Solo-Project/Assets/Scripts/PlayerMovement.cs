using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputSystem_Actions inputActions;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Transform orientation;

    private Vector2 movementInput;
    private Vector3 movementDirection;
    private Vector3 appliedForce;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        movementInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 velocity = new Vector3(movementInput.x * movementSpeed, rb.linearVelocity.z, movementInput.y * movementSpeed);
        rb.linearVelocity = transform.TransformDirection(velocity);
        
        
        // movementInput.y = 0f;
        // movementDirection = orientation.forward + orientation.right * movementInput.x;
        //
        // appliedForce = movementDirection.normalized * (movementSpeed * 10);
        // rb.AddForce(appliedForce, ForceMode.Force);
        //rb.linearVelocity = movementInput * movementSpeed;
    }

    private void OnMove(InputValue value)
    {
        //movementInput = value.Get<Vector2>();
    }
}
