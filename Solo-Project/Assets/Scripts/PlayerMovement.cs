using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputSystem_Actions inputActions;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Transform orientation;
    [SerializeField] private float playerHeight;
    [SerializeField] private Transform cameraTransform;
    
    public LayerMask whatIsGround;
    bool grounded =false;

    private Vector2 movementInput;
    private Vector3 movementDirection;
    private Vector3 appliedForce = Vector3.zero;

    private void Start()
    {
        inputActions = InputManager.instance.GetInputActions();
    }

    public bool GetIsGrounded()
    {
        return grounded;
    }

    public float GetGroundDrag()
    {
        return 0.0f;
    }

    public Vector3 GetCurrentAppliedForce()
    {
        return appliedForce;
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        movementInput = inputActions.Player.Move.ReadValue<Vector2>();
        Transform newOrientation = orientation;
        newOrientation.forward = new Vector3(cameraTransform.forward.x, newOrientation.forward.y, cameraTransform.forward.z);
        movementDirection = (newOrientation.forward * movementInput.y + cameraTransform.right * movementInput.x).normalized;
        appliedForce = movementDirection * movementSpeed; 
        rb.linearVelocity = appliedForce;
        
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
