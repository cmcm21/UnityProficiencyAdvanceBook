using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
   [SerializeField] private float moveSpeed;
   [SerializeField] private float rotationSpeed;
   
   private Rigidbody m_rigidBody;
   private Vector3 m_verticalInput;
   private float m_horizontalInput;
   private Vector3 m_eulerAngleVelocity;
   private Quaternion deltaRotation;

   private void Start()
   {
      m_rigidBody = GetComponent<Rigidbody>();
   }

   private void FixedUpdate()
   {
      GetInput();
      CalculateVelocities();
      //m_rigidBody.MovePosition(transform.position + m_verticalInput * Time.deltaTime);
      m_rigidBody.velocity = m_verticalInput * moveSpeed;
      
      m_rigidBody.MoveRotation(transform.rotation * deltaRotation);
   }

   private void GetInput()
   {
      var vInput = Input.GetAxis("Vertical");
      m_verticalInput = transform.forward * vInput;
      
      m_horizontalInput = Input.GetAxis("Horizontal");
   }

   private void CalculateVelocities()
   {
      m_eulerAngleVelocity = new Vector3(0, rotationSpeed * m_horizontalInput, 0);
      deltaRotation = Quaternion.Euler(m_eulerAngleVelocity * Time.fixedDeltaTime);
   }
   
}
