using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateMazeRandom : MonoBehaviour
{
    private const int WallHeight = 4;
    
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
        }
        else
            Debug.LogError($"[{GetType()}]Ground or ceiling were not found");
    }


    private void Init()
    {
        horizontalWall.transform.localScale = new Vector3(wallSize, WallHeight, 0.1f);
        verticalWall.transform.localScale = new Vector3(0.1f, WallHeight, wallSize);
        _ground.transform.localScale = new Vector3((width + 1) * wallSize, 1, (height + 1) * wallSize);

        _grid = new Compass[width, height];
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
                if(i > 0) // we remove the vertical wall starting in 0 to can form a perfect square grid maze
                    _gridObjectsVertical[j, i] = InstantiateWall(true, i, j, xOffset, zOffset);
                if(j > 0) // we remove the horizontal wall starting in 0 to can form a perfect square grid maze
                    _gridObjectsHorizontal[j, i] = InstantiateWall(false, i, j, xOffset, zOffset);
            }
        }
    }

    private GameObject InstantiateWall(bool vertical, int i, int j,float xOffset, float zOffset)
    {
        GameObject wall;
        if (vertical)
        {
            Vector3 wallPosition = new Vector3(
                wallSize / 2.0f + j * wallSize + xOffset,
                wallSize / 2.0f,
                // in vertical wall axe z has a size of 0.1 that why it doesn't need sum of the wall size here
                i * wallSize + zOffset
            );
            wall = Instantiate(verticalWall, wallPosition, Quaternion.identity,wallContainer.transform);
            wall.name = $"vertical wall({i},{j})";
        }
        else
        {
            Vector3 wallPosition = new Vector3(
                // in horizontal wall axe x has a size of 0.1 that why it doesn't need sum of the wall size here
                j * wallSize + xOffset, 
                wallSize / 2.0f,
                wallSize / 2.0f + i * wallSize + zOffset
            );
            wall = Instantiate(horizontalWall, wallPosition, Quaternion.identity,wallContainer.transform);
            wall.name = $"horizontal wall({i},{j})";
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
        bool onBondaries = false;
        carvingDirection = Compass.NONE;
        if (column == width - 1) // if we have reached most right wall
        {
            onBondaries = true;
            if (row < height - 1) // if we haven't reached most up wall
                carvingDirection = Compass.NORTH;
            else
                carvingDirection = Compass.WEST;
        }

        if (row == height - 1) // if we reach have reached most up wall
        {
            onBondaries = true;
            if (column < width - 1) // if we haven't reached most right wall
                carvingDirection = Compass.EAST;
            else
                carvingDirection = Compass.SOUTH;
        }
        return onBondaries;
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
                    _gridObjectsVertical[column - 1,row]?.SetActive(false);
                else if(tile == Compass.SOUTH)
                    _gridObjectsVertical[column,row - 1]?.SetActive(false);
            }
        }
    }
}
