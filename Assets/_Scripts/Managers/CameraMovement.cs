using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float _horzInput;
    private float _vertInput;
    [SerializeField, Range(0,20)] private float _moveSpeed;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    void LateUpdate()
    {
        HandleCamMovement();    
    }

    public void HandleCamMovement()
    {
        // Handle camera movement with WASD
        _horzInput = Input.GetAxis("Horizontal");
        _vertInput = Input.GetAxis("Vertical");

        //Return early to avoid unecessary calculations
        if (_horzInput == 0f && _vertInput == 0f) { return; }

        Vector3 movementDirection = new(_horzInput, _vertInput, -1);
        transform.position += _moveSpeed * Time.deltaTime * movementDirection;

        // Clamp the camera position to prevent it from moving outside the specified range
        Vector3 clampedPosition = new(
            Mathf.Clamp(transform.position.x, _minPosition.x, _maxPosition.x),
            Mathf.Clamp(transform.position.y, _minPosition.y, _maxPosition.y),
            -1
        );

        // Update the camera's position to the clamped position
        transform.position = clampedPosition;
    }
}
