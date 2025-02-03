using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputSystem_Actions inputActions;
    
    private void OnEnable()
    {
        InputManager.OnPlayerMovement += UpdateMovement;
    }

    private void OnDisable()
    {
        InputManager.OnPlayerMovement -= UpdateMovement;
    }

    private void UpdateMovement(InputAction.CallbackContext context)
    {
        
    }
}
