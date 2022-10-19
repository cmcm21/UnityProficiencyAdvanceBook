using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum TerrainType {WALL = 1, GROUND = 0}
[CreateAssetMenu(fileName = "MazeData", menuName = "Data/Maze Data", order = 1)]
public class MazeData : ScriptableObject
{
    [SerializeField] private int worldMapSize;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int worldMapSquare;
    public int WorldMapSquare => worldMapSquare;
    
    [SerializeField] private List<TerrainType> worldMap;

    public List<TerrainType> WorldMap => worldMap;
   
    public void UpdateWorldMap(int index, TerrainType terrainType)
    {
        worldMap[index] = terrainType;
    }

    public void SetWordList(List<TerrainType> list)
    {
        worldMap = list;
    }
}
