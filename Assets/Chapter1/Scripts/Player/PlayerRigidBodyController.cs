using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRigidBodyController : MonoBehaviour
{
   [SerializeField] private Transform feet;
   [SerializeField] private LayerMask groundLayer;
   [Space]
   [SerializeField] private float moveSpeed;
   [SerializeField] private float rotationSpeed;
   [SerializeField] private float jumpForce;
   
   private Rigidbody _rigidBody;
   private Vector3 _verticalInput;
   private float _horizontalInput;
   private Vector3 _eulerAngleVelocity;
   private Quaternion _deltaRotation;
   private bool _jump;

   private void Start()
   {
      _rigidBody = GetComponent<Rigidbody>();
   }

   private void FixedUpdate()
   {
      GetInput();
      CalculateVelocities();
      Move();
      if(CanJump())
         Jump();
   }

   private void Move()
   {
      //m_rigidBody.MovePosition(transform.position + m_verticalInput * Time.deltaTime);
      _rigidBody.velocity = new Vector3(_verticalInput.x, _rigidBody.velocity.y, _verticalInput.z);
      _rigidBody.MoveRotation(transform.rotation * _deltaRotation);
   }

   private void GetInput()
   {
      var vInput = Input.GetAxis("Vertical");
      _verticalInput = transform.forward * vInput * moveSpeed;
      _verticalInput.y = 0.0f;

      _horizontalInput = Input.GetAxis("Horizontal") + Input.GetAxis("Mouse X");
      _jump = Input.GetKeyDown(KeyCode.Space);
   }

   private void CalculateVelocities()
   {
      _eulerAngleVelocity = new Vector3(0, rotationSpeed * _horizontalInput, 0);
      _deltaRotation = Quaternion.Euler(_eulerAngleVelocity * Time.fixedDeltaTime);
   }

   private void Jump()
   {
      _rigidBody.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
   }

   private bool CanJump()
   {
      return _jump && Physics.CheckSphere(feet.position, 0.1f, groundLayer);
   }
}
