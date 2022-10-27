using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public delegate void MazeDataLoaded(MazeData data);

public static class DataManager
{
    private static AsyncOperationHandle<MazeData> _mazeDataHandleOpt;
    public static event MazeDataLoaded OnMazeDataLoaded;
    
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
}
