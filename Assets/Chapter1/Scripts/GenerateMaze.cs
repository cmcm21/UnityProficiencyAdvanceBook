using System;
using System.Collections.Generic;
using Chapter1;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Chapter1
{
    public class GenerateMaze : MonoBehaviour
    {
        public enum GenerationOption {FROM_SCRIPTABLE,FROM_XML,FROM_IMAGE}
        [SerializeField] private Transform wallsContainer;
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GenerationOption generateMazeOption;
        [SerializeField] private AssetReference mazeDataRef;
        [SerializeField] private Texture2D mazeImage;
        

        private MazeData _mazeData;
        private List<GameObject> _walls = new List<GameObject>();

        private void Start()
        {
            Load();
        }

        public void SetMazeDataRef(AssetReference newData)
        {
            mazeDataRef = newData;
        }

        public void SetGenerationOption(GenerationOption option)
        {
            generateMazeOption = option;
        }

        private void Load()
        {
             if (generateMazeOption == GenerationOption.FROM_IMAGE ||
                generateMazeOption == GenerationOption.FROM_SCRIPTABLE) {
                DataManager.LoadMaze(mazeDataRef);
                DataManager.OnMazeDataLoaded += DataManager_OnMazeDataLoaded;
             }
             else if (generateMazeOption == GenerationOption.FROM_XML)
             {
                 DataManager.LoadMazeXml(mazeDataRef);
                 DataManager.OnMazeXmlDataLoaded += DataManager_OnMazeXmlDataLoaded;
             }           
        }


        private void DataManager_OnMazeDataLoaded(MazeData data)
        {
            _mazeData = data;
            if(generateMazeOption == GenerationOption.FROM_IMAGE)
                LoadMazeFromImage();
            else if(generateMazeOption == GenerationOption.FROM_SCRIPTABLE)
                LoadMazeFromScriptableData();
        }

        private void LoadMazeFromScriptableData()
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

        private void DataManager_OnMazeXmlDataLoaded(MazeXMLData data)
        {
            LoadFromXml(data);
        }

        private void LoadFromXml(MazeXMLData data)
        {
            var tileSeparation = data.TileSize;
            var centerOffset = data.MapSize / 2;
            for (int i = -data.MapSize; i <= data.MapSize; i++)
            {
                var plus = i >= 0 ? 1 : -1;
                int x = i % data.TileSize + plus;
                int y = i / data.TileSize + plus;
                
                if (i <= 0)
                    centerOffset = data.MapSize / 2 - data.MapSize;
                else
                    centerOffset = data.MapSize / 2;
                
                if (Mathf.Abs(x) == (int)data.Location.x || Mathf.Abs(y) == (int)data.Location.z)
                    InstantiateWall(x,y,tileSeparation,centerOffset);
            }
        }

        private void InstantiateWall(int x, int y,int tileSeparation,int centerOffset)
        {
            var position = new Vector3(
                centerOffset - (x * tileSeparation),
                1.5f,
                centerOffset - (y * tileSeparation)
                );
            
            Debug.Log($"<color=red>[{GetType()}]:: cords with wall: ({x} , {y})" +
                      $" Position: {position}</color>");
            
            var wall = Instantiate(
                wallPrefab,
                position,
                Quaternion.identity,
                wallsContainer
            );
        
            _walls.Add(wall);
        }
    }
}
