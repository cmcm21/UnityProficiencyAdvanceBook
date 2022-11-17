using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateMazeRandom : MonoBehaviour
{
    private const int WallHeight = 4;

    [SerializeField] private GameObject player;
    
    [Header("Grid size")]
    [SerializeField] [Range(5, 100)] 
    private int width, height;

    [SerializeField] [Range(5, 100)]
    private int wallSize;
    
    [SerializeField]
    private GameObject horizontalWall,verticalWall,wallContainer;
    

    private GameObject[,] _gridObjectsHorizontal; 
    private GameObject[,] _gridObjectsVertical;


    enum Compass
    {
        NORTH = 1,
        SOUTH = 2,
        EAST = 3,
        WEST = 4,
        NONE = -1
    };

    private Compass[,] _grid;

    private Transform _ground;
    private Transform _ceiling;

    private void Start()
    {
        _ground = GameObject.FindWithTag("Ground").transform;
        //_ceiling = GameObject.FindWithTag("Ceiling").transform;

        if (_ground != null) //&& _ceiling != null)
        {
            Init(); 
            GenerateMazeBinary();
            RemoveWalls();
            AddPlayer();
        }
        else
            Debug.LogError($"[{GetType()}]Ground or ceiling were not found");
    }


    private void Init()
    {
        horizontalWall.transform.localScale = new Vector3(wallSize, WallHeight, 0.1f);
        verticalWall.transform.localScale = new Vector3(0.1f, WallHeight, wallSize);
        _ground.transform.localScale = new Vector3((width + 1) * wallSize, 1, (height + 1) * wallSize);

        _grid = new Compass[width+1, height+1];
        _gridObjectsHorizontal = new GameObject[width + 1, height + 1];
        _gridObjectsVertical = new GameObject[width + 1, height + 1];
        
        DrawFullGrid();
    }

    private void DrawFullGrid()
    {
        float xOffset = -(width * wallSize) / 2.0f;
        float zOffset = -(height * wallSize) / 2.0f;
        
        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j <= width; j++)
            {
                if(i < height) 
                    _gridObjectsVertical[j, i] = InstantiateWall(true, i, j, xOffset, zOffset);
                if(j < width) 
                    _gridObjectsHorizontal[j, i] = InstantiateWall(false, i, j, xOffset, zOffset);
            }
        }
    }

    private GameObject InstantiateWall(bool vertical, int i, int j,float xOffset, float zOffset)
    {
        GameObject wall;
        if (vertical)
        {
            float verticalWallSize = verticalWall.transform.localScale.z;
            Vector3 wallPosition = new Vector3(
                -verticalWallSize / 2.0f + j * verticalWallSize + xOffset,
                wallSize / 2.0f,
                i * verticalWallSize + zOffset
            );
            wall = Instantiate(verticalWall, wallPosition, Quaternion.identity,wallContainer.transform);
            wall.name = $"vertical wall({i},{j})";
            wall.tag = "Wall";
        }
        else
        {
            float horizontalWallSize = horizontalWall.transform.localScale.x;
            Vector3 wallPosition = new Vector3(
                j * horizontalWallSize + xOffset, 
                wallSize / 2.0f,
                -(horizontalWallSize / 2.0f) + i * horizontalWallSize + zOffset
            );
            wall = Instantiate(horizontalWall, wallPosition, Quaternion.identity,wallContainer.transform);
            wall.name = $"horizontal wall({i},{j})";
            wall.tag = "Wall";
        }
        return wall;
    }

    private void GenerateMazeBinary()
    {
        float randomNumber;
        Compass carvingDirection;
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                randomNumber = Random.Range(0, 100);
                if (randomNumber > 30)
                    carvingDirection = Compass.NORTH;
                else
                    carvingDirection = Compass.EAST;

                if (CheckGridBoundariesBinaryGeneration(row, column, out var newDirection))
                    carvingDirection = newDirection;

                _grid[column, row] = carvingDirection;
            }
        }
    }

    private bool CheckGridBoundariesBinaryGeneration(int row, int column, out Compass carvingDirection)
    {
        bool onBoundaries = false;
        carvingDirection = Compass.NONE;
        if (column == width - 1) // if we have reached most right wall
        {
            onBoundaries = true;
            if (row < height - 1) // if we haven't reached most up wall
                carvingDirection = Compass.NORTH;
            else
                carvingDirection = Compass.WEST;
        }

        else if (row == height - 1) // if we reach have reached most up wall
        {
            onBoundaries = true;
            if (column < width - 1) // if we haven't reached most right wall
                carvingDirection = Compass.EAST;
            else
                carvingDirection = Compass.SOUTH;
        }
        return onBoundaries;
    }

    private void RemoveWalls()
    {
        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Compass tile = _grid[column, row];
                if(tile == Compass.NORTH)
                    _gridObjectsHorizontal[column,row + 1]?.SetActive(false);
                else if(tile == Compass.EAST)
                    _gridObjectsVertical[column + 1, row]?.SetActive(false);
                else if(tile == Compass.WEST)
                    _gridObjectsVertical[column, row + 1]?.SetActive(false);
            }
        }
    }

    private void AddPlayer()
    {
        var xOffset = -(width * wallSize) / 2;
        var zOffset = -(height * wallSize) / 2;

        player.transform.position = new Vector3(xOffset, 3.0f, zOffset);
    }
}