using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private float rotationalSpeed = 10f;
    [SerializeField] private float orbitalSpeed = 0.20f;
    [SerializeField] private float orbitalAngle = 0.0f;
    [SerializeField] private float angle = 0.0f;
    [SerializeField] private float orbitalRotationalSpeed = 20;
    [SerializeField] private float distanceToSun = 150;
    [SerializeField] private Color orbitColor = Color.blue;
    [SerializeField] private int lenghtOfOrbitRenderer = 100;
    
    private GameObject _sun;

    public void SetData(XmlNode data)
    {
       string rawName, rawRadius, rawDistanceToSun, rawRotationPeriod, rawOrbitalVelocity,rawColor;
       rawName = data.Attributes.GetNamedItem("name").Value;
       rawRadius = data.Attributes.GetNamedItem("radius").Value;
       rawDistanceToSun = data.Attributes.GetNamedItem("distanceToSun").Value;
       rawRotationPeriod = data.Attributes.GetNamedItem("rotationPeriod").Value;
       rawOrbitalVelocity = data.Attributes.GetNamedItem("orbitalVelocity").Value;
       rawColor = data.Attributes.GetNamedItem("color").Value;
       
       orbitalSpeed *= float.Parse(rawOrbitalVelocity);
       var rotationSpeed = 1 / float.Parse(rawRotationPeriod);
       
       rotationalSpeed *= rotationSpeed;
       distanceToSun *= float.Parse(rawDistanceToSun);

       var radius = float.Parse(rawRadius);
       transform.localScale = new Vector3(radius, radius, radius);
       
       this.name = rawName;
       transform.Find("Label").GetComponent<TextMesh>().text = rawName;
       orbitColor = ValueConverter.Instance.ToColor(rawColor);
    }
    
    private void Start()
    {
       _sun = GameObject.FindWithTag("Sun");
       transform.position = new Vector3(distanceToSun, 0, distanceToSun);
       DrawOrbit();
    }

    private void Update()
    { 
       RotatePlanet(); 
       MovePlanet();
    }

    private void DrawOrbit()
    {
       LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
       lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));

       lineRenderer.startColor = orbitColor;
       lineRenderer.endColor = orbitColor;
       lineRenderer.startWidth = 1.0f;
       lineRenderer.endWidth = 1.0f;
       lineRenderer.positionCount = lenghtOfOrbitRenderer + 1;

       float unitAngle = (2 * Mathf.PI) / lenghtOfOrbitRenderer;// angle of each vertex to made a circle
       for (int i = 0; i <= lenghtOfOrbitRenderer; i++)
       {
          float currentAngle = unitAngle * i;
          
          Vector3 vertex = new Vector3(
             distanceToSun * Mathf.Cos(currentAngle), // cos to form a circle
             0,
             distanceToSun * (float)Math.Sin(currentAngle) // sin to form a circle
          );
          
          lineRenderer.SetPosition(i, vertex);
       }
    }


    private void RotatePlanet()
    {
       transform.Rotate(
          Vector3.up,
          rotationalSpeed * Time.deltaTime,
          Space.World
          );
    }

    private void MovePlanet()
    {
       Vector3 newPosition;
       
       orbitalAngle += Time.deltaTime * orbitalSpeed;

       newPosition.x = _sun.transform.position.x + distanceToSun * Mathf.Cos(orbitalAngle);
       newPosition.z = _sun.transform.position.z + distanceToSun * Mathf.Sin(orbitalAngle);
       newPosition.y = _sun.transform.position.y;

       transform.position = newPosition;
    }
}
