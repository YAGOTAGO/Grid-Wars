using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float _horzInput;
    private float _vertInput;
    private Camera _camera;

    [Header("WASD movement")]
    [SerializeField, Range(0,20)] private float _moveSpeed;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    [Header("Zoom Movement")]
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _minOrthoSize = 2f;
    [SerializeField] private float _maxOrthoSize = 10f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        HandleCamMovement(); 
        HandleScrollMovement();
    }

    public void HandleScrollMovement()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float newOrthoSize = _camera.orthographicSize - scrollInput * _zoomSpeed;
            newOrthoSize = Mathf.Clamp(newOrthoSize, _minOrthoSize, _maxOrthoSize);
            _camera.orthographicSize = newOrthoSize;
        }
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
