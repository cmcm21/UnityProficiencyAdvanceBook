using System;
using System.Collections.Generic;
using UnityEngine;


public enum TerrainType {GROUND = 0,WALL=1, SAND=2}
[CreateAssetMenu(fileName = "MazeData", menuName = "Data/Maze Data", order = 1)]
public class MazeData : ScriptableObject
{
    [SerializeField] private int worldMapSize;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int worldMapSquare;
    public int WorldMapSquare => worldMapSquare;
    
    [SerializeField] private List<TerrainType> worldMap;
    public List<TerrainType> WorldMap => worldMap;

    [SerializeField] private List<TerrainEnumXTexture> terrainEnumXTextures;

    [SerializeField] private int tileSize;
    public int TileSize => tileSize;

    private Dictionary<TerrainType, Texture> _terrainEnumTextDict;
    public Dictionary<TerrainType, Texture> terrainEnumTexDict
    {
        get
        {
            if (_terrainEnumTextDict == null || _terrainEnumTextDict.Count == 0)
            {
                UpdateTerrainDict();
                return _terrainEnumTextDict;
            }
            return _terrainEnumTextDict;
        }
    }

    public void UpdateTerrainDict()
    {
        _terrainEnumTextDict = new Dictionary<TerrainType, Texture>();
        foreach (var terrainXText in terrainEnumXTextures)
        {
            if(!_terrainEnumTextDict.ContainsKey(terrainXText.type))
                _terrainEnumTextDict.Add(terrainXText.type,terrainXText.texture);
            else
                Debug.LogWarning($"[{GetType()}]:: type : {terrainXText.type} already exists in dict");
        }
    }

    public void UpdateWorldMap()
    {
        for(int i = 0; i < worldMap.Count; i++)
        {
            if (!_terrainEnumTextDict.ContainsKey(worldMap[i]))
                worldMap[i] = TerrainType.GROUND;
        }
    }
   
    public void UpdateWorldMapNext(int index, TerrainType terrainType)
    {
        TerrainType newType;
        var type = (int)terrainType;
        try {
            newType = (TerrainType)(++type);
        }
        catch (Exception e) {
            newType = TerrainType.GROUND;
        }

        if (_terrainEnumTextDict != null && _terrainEnumTextDict.ContainsKey(newType))
            worldMap[index] = newType;
        else
            worldMap[index] = TerrainType.GROUND;
    }

    public void SetWordList(List<TerrainType> list)
    {
        worldMap = list;
    }

    public void ResetWordMap()
    {
        worldMap.Clear();
        if(terrainEnumTexDict != null)
            terrainEnumTexDict.Clear();
    }
}

[System.Serializable]
public class TerrainEnumXTexture
{
    public TerrainType type;
    public Texture2D texture;
}
