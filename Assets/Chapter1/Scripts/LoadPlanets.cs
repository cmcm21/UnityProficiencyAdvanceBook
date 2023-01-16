using System;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlanets : MonoBehaviour
{
    [SerializeField] private TextAsset data;
    [SerializeField] private GameObject planetPrefab;

    private List<Planet> _planets = new List<Planet>();

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        if (data == null) return;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(data.text);
        var nodes = xmlDocument.SelectNodes("planets/planet");
        
        if (nodes == null || nodes.Count == 0) return;

        var container = new GameObject("Planets");

        foreach (XmlNode planet in nodes)
            InstantiatePlanet(planet,container);
    }

    private void InstantiatePlanet(XmlNode xmlData,GameObject container)
    {
        var planetGO = Instantiate(
            planetPrefab, Vector3.zero, Quaternion.identity, container.transform);

        var planet = planetGO.GetComponent<Planet>();
        planet.SetData(xmlData);
    }
}
