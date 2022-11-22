using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GenerateHeightMap : MonoBehaviour
{
    [SerializeField] [Range(10, 100)] 
    private int mapHeight, mapWidth;

    [SerializeField] [Range(0, 100)] 
    private float blockSize, blockHeight, frequency, scale;

    [SerializeField] private GameObject minecraftBlock;
    [SerializeField] private Transform environmentContainer;
    [SerializeField] private GameObject player;
    
    private float [,]map;
    private void Start()
    {
        Init();     
        DisplayArray();
        InstantiatePlayer();
    }

    private void Init()
    {
        map = new float[mapWidth, mapHeight];
        minecraftBlock.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
        InitArray();
    }

    private void InitArray()
    {
        for (int j = 0; j < mapHeight; j++)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                float nx = i / mapWidth;
                float ny = j / mapHeight;
                map[i, j] = Mathf.PerlinNoise(
                    i * 1.0f / frequency + 0.1f,
                    j*1.0f/frequency  + 0.1f
                );
            }
        }
    }

    private void DisplayArray()
    {
        for (int j = 0; j < mapHeight; j++)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                GameObject t = (GameObject)Instantiate(
                    minecraftBlock,
                    new Vector3(i * blockSize,Mathf.Round(map[i,j] * blockHeight * scale),j*blockSize),
                    Quaternion.identity,
                    environmentContainer
                );
            }
        }
    }

    private void InstantiatePlayer()
    {
        var playerGameObject = Instantiate(
            player, 
            new Vector3((float)mapWidth / 2, 10, (float)mapHeight/2), 
            Quaternion.identity
        );
    }
}
