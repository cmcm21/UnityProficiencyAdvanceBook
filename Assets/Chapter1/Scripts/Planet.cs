using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private float _rotationalSpeed = 10f;
    private float _orbitalSpeed = 0.20f;
    private float _orbitalAngle = 0.0f;
    private float _angle = 0.0f;
    private float _orbitalRotatinalSpeed = 20;
    private float _distanceToSun = 150;
    
    private GameObject _sun;
    
    private void Start()
    {
       _sun = GameObject.FindWithTag("Sun");
       transform.position = new Vector3(_distanceToSun, 0, _distanceToSun);
    }

    private void Update()
    { 
       RotatePlanet(); 
       MovePlanet();
    }


    private void RotatePlanet()
    {
       transform.Rotate(
          Vector3.up,
          _rotationalSpeed * Time.deltaTime,
          Space.World
          );
    }

    private void MovePlanet()
    {
       Vector3 newPosition;
       
       _orbitalAngle += Time.deltaTime * _orbitalSpeed;

       newPosition.x = _sun.transform.position.x + _distanceToSun * Mathf.Cos(_orbitalAngle);
       newPosition.z = _sun.transform.position.z + _distanceToSun * Mathf.Sin(_orbitalAngle);
       newPosition.y = _sun.transform.position.y;

       transform.position = newPosition;
    }
}
