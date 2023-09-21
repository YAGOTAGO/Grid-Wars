using UnityEngine;

public class StayWithCamera : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _priorPos;
    [SerializeField] private int _offset;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 currPos =_camera.transform.position;

        if(_priorPos != currPos )
        {
            currPos = _camera.transform.position;
            _priorPos = currPos;
            
            //use z 0 so can see item
            currPos.z = 0;
            currPos.y += _offset;
            transform.position = currPos; 
        }
    }
}
