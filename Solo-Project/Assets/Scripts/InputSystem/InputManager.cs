using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions inputActions;

    public static InputManager instance;

    private void Awake()
    {
        instance = this;
        inputActions = new InputSystem_Actions();
    }

    public InputSystem_Actions GetInputActions()
    {
        return inputActions;
    }
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
