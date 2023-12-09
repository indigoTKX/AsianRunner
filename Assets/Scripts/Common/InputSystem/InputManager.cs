using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager : MonoBehaviour
{
    public event Action<Vector2> OnMoved;
    public event Action OnAnyTap;

    private TouchControls _touchControls;
    private float _screenCenterX;
    private float _screenCenterY;
    
    private void Awake()
    {
        _touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        _touchControls.Enable();
    }

    private void OnDisable()
    {
        _touchControls.Disable();
    }

    private void Start()
    {
        _touchControls.Gameplay.Move.performed += HandleMoveInput;
        _touchControls.Gameplay.AnyTap.started += HandleAnyTap;
        
        _screenCenterX = Screen.width / 2.0f;
        _screenCenterY = Screen.height / 2.0f;
    }

    private void HandleMoveInput(InputAction.CallbackContext inputContext)
    {
        // var screenPosition = inputContext.ReadValue<Vector2>();
        // var inputVector = new Vector2
        // {
        //     x = (screenPosition.x - _screenCenterX) / _screenCenterX,
        //     y = (screenPosition.y - _screenCenterY) / _screenCenterY
        // };
        
        var inputVector = inputContext.ReadValue<Vector2>();
        inputVector.Normalize();
        OnMoved?.Invoke(inputVector);
    }

    private void HandleAnyTap(InputAction.CallbackContext inputContext)
    {
        OnAnyTap?.Invoke();
    }
}
