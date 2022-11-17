using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform[] camerasPositions;
    [SerializeField] private float sensitivity;
    [SerializeField] private float returnSpeed;

    private Vector2 _mouseInput;
    private float _xRot;
    private Quaternion _originalRotation;
    private bool _changeCamera;
    private int _currentCamera;

    private void Start()
    {
        _currentCamera = 0;
        _originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
       GetInput(); 
       //MoveCamera();
       if(_changeCamera)
           ChangeCamera();
    }

    private void GetInput()
    {
        _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _changeCamera = Input.GetKeyDown(KeyCode.Space);
    }

    private void MoveCamera()
    {
        _xRot = _mouseInput.y * sensitivity;
        transform.localRotation = Quaternion.Euler(0.0f,_xRot,0.0f);
        //StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        float time = 0;
        while (time < returnSpeed)
        {
            time += Time.deltaTime;
            var rotation = Quaternion.Lerp(transform.rotation, _originalRotation, time/returnSpeed);
            transform.rotation = rotation;
            yield return null;
        }
        
        transform.rotation = _originalRotation;
        yield return null;
    }

    private void ChangeCamera()
    {
        _currentCamera++;
        _currentCamera %= camerasPositions.Length;
        transform.position = camerasPositions[_currentCamera].position;
        transform.rotation = camerasPositions[_currentCamera].rotation;

        _originalRotation = camerasPositions[_currentCamera].rotation;
    }
}
