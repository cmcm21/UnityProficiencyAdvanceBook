using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform[] camerasPositions;
    [SerializeField] private float sensitivity;
    [SerializeField] private float returnSpeed;

    private Vector2 _mouseInput;
    private float _xRot;
    private float _yRot;
    private bool _changeCamera;
    private int _currentCamera;

    private void Start()
    {
        _currentCamera = 0;
    }

    private void Update()
    {
       GetInput(); 
       
       if(_changeCamera)
           ChangeCamera();
       
       MoveCamera();
    }

    private void GetInput()
    {
        _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _changeCamera = Input.GetKeyDown(KeyCode.J);
    }

    private void MoveCamera()
    {
        _xRot -= _mouseInput.y * sensitivity;
        _xRot = Mathf.Clamp(_xRot, -45f, 45f);
        
        transform.localRotation = Quaternion.Euler(_xRot,0.0f,0.0f);
    }

    private void ChangeCamera()
    {
        _currentCamera++;
        _currentCamera %= camerasPositions.Length;
        transform.position = camerasPositions[_currentCamera].position;
        transform.rotation = camerasPositions[_currentCamera].rotation;
        
    }
}
