using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [Space] 
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float sensitivity;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController _controller;
    private Vector3 _playerMovement;
    private Vector2 _mouseInput;
    private Vector3 _velocity;
    private float _xRot;
    
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
       GetInput(); 
       MovePlayer();
       MoveCamera();
    }

    private void GetInput()
    {
        _playerMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void MovePlayer()
    {
        var moveVector = transform.TransformDirection(_playerMovement);

        if (_controller.isGrounded)
        {
            _velocity.y = -1;
            if (Input.GetKeyDown(KeyCode.Space))
                _velocity.y = jumpForce;
        }
        else
            _velocity.y -= gravity * -2f * Time.deltaTime;

        _controller.Move(moveVector * Time.deltaTime * speed);
        _controller.Move(_velocity * Time.deltaTime); // only moves y axe
    }

    private void MoveCamera()
    {
        _xRot -= _mouseInput.y * sensitivity;
        _xRot = Mathf.Clamp(_xRot, -45f, 45f);
        transform.Rotate(0f,_mouseInput.x * sensitivity, 0f);
        playerCamera.localRotation = Quaternion.Euler(_xRot,0f,0f);
    }
}
