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
    private bool currentlyTransforming = false;

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

    public void ToggleMovement(bool toggle)
    {
        currentlyTransforming = toggle;
        
        if (toggle)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
        
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
        if (!currentlyTransforming)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            movementInput = inputActions.Player.Move.ReadValue<Vector2>();
            Transform newOrientation = orientation;
            newOrientation.forward = new Vector3(cameraTransform.forward.x, newOrientation.forward.y, cameraTransform.forward.z);
            movementDirection = (newOrientation.forward * movementInput.y + cameraTransform.right * movementInput.x).normalized;
            appliedForce = movementDirection * movementSpeed; 
            rb.linearVelocity = appliedForce;
        }
    }
}
