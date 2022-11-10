using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Debug = UnityEngine.Debug;

namespace Chapter1
{
    public delegate void MazeScriptableDataLoaded(MazeData data);
    public delegate void MazeXmlDataLoaded(MazeXMLData data);

    public static class DataManager
    {
        private static AsyncOperationHandle<MazeData> _mazeDataHandleOpt;
        private static AsyncOperationHandle<TextAsset> _mazeXmlDataHandleOpt;
        public static event MazeScriptableDataLoaded OnMazeDataLoaded;
        public static event MazeXmlDataLoaded OnMazeXmlDataLoaded;
    
        public static void LoadMaze(AssetReference mazeRef)
        {
            _mazeDataHandleOpt = Addressables.LoadAssetAsync<MazeData>(mazeRef);
            _mazeDataHandleOpt.Completed += MazeDataHandleOptOnCompleted;
        
        }

        private static void MazeDataHandleOptOnCompleted(AsyncOperationHandle<MazeData> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
                OnMazeDataLoaded?.Invoke(obj.Result);
        }

        public static void LoadMazeXml(AssetReference mazeXmlRef)
        {
            _mazeXmlDataHandleOpt = Addressables.LoadAssetAsync<TextAsset>(mazeXmlRef);
            _mazeXmlDataHandleOpt.Completed += MazeXmlDataHandleOptOnCompleted;
        }

        private static void MazeXmlDataHandleOptOnCompleted(AsyncOperationHandle<TextAsset> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(obj.Result.text);
                    MazeXMLData xmlData = new MazeXMLData(document);
                    
                    if(xmlData.Initialized)
                        OnMazeXmlDataLoaded?.Invoke(xmlData);
                }
                catch (Exception e) {
                   Debug.LogWarning($"[{typeof(DataManager)}]:: Error trying to read XML file: {e.Message}");
                }
            }
        }
    }
}