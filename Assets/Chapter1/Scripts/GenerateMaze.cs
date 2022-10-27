using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GenerateMaze : MonoBehaviour
{
    [SerializeField] private Transform wallsContainer;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private AssetReference mazeDataRef;
    [SerializeField] private bool loadMazeFromImage;
    [SerializeField] private Texture2D mazeImage;

    private MazeData _mazeData;
    private List<GameObject> walls;

    private void Awake()
    {
        walls = new List<GameObject>();
    }

    private void Start()
    {
        DataManager.LoadMaze(mazeDataRef);
        DataManager.OnMazeDataLoaded += DataManagerOnOnMazeDataLoaded;
    }

    private void DataManagerOnOnMazeDataLoaded(MazeData data)
    {
        _mazeData = data;
        if(loadMazeFromImage)
            LoadMazeFromImage();
        else
            LoadMazeFromData();
    }

    private void LoadMazeFromData()
    {
        var tileSeparation = _mazeData.TileSize;
        var centerOffset = (_mazeData.WorldMapSize / 2) - (_mazeData.WorldMapSquare / 2);
        for (int i = 0; i < _mazeData.WorldMapSize; i++)
        {
            if (_mazeData.WorldMap[i] == TerrainType.WALL)
            {
                int x = i % _mazeData.WorldMapSquare;
                int y = i / _mazeData.WorldMapSquare; 
                InstantiateWall(x,y,tileSeparation,centerOffset);
            }
        }       
    }

    private void LoadMazeFromImage()
    {
        if (mazeImage == null) return;
        var worldMap = new Color[mazeImage.width, mazeImage.height];
        var tileSeparation = _mazeData.TileSize;
        var centerOffset = (mazeImage.width / 2) * tileSeparation; 
        
        for (int x = 0; x < mazeImage.width; x++)
        {
            for (int y = 0; y < mazeImage.height; y++)
            {
                worldMap[x,y] = mazeImage.GetPixel(x, y);
                if (worldMap[x, y] != Color.white)
                    InstantiateWall(x,y,tileSeparation,centerOffset);
            }
        }
    }

    private void InstantiateWall(int x, int y,int tileSeparation,int centerOffset)
    {
        var wall = Instantiate(
            wallPrefab,
            new Vector3(centerOffset - (x * tileSeparation), 1.5f, centerOffset -(y * tileSeparation)),
            Quaternion.identity,
            wallsContainer
        );
        
        walls.Add(wall);
    }
}
