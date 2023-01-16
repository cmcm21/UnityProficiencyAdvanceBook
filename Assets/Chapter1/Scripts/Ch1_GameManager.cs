using System;
using System.Collections;
using System.Collections.Generic;
using Chapter1;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Ch1_GameManager : MonoBehaviour
{
    [SerializeField] private GameObject environmentPrefab;
    [SerializeField] private AssetReference option1;
    [SerializeField] private AssetReference option2;
    [SerializeField] private AssetReference option3;

    private Dictionary<int, AssetReference> _dictionary;
    private Ch1_UIManager _uiManager;

    private void Awake()
    {
        _dictionary = new Dictionary<int, AssetReference>()
        {
            {1,option1},
            {2,option2},
            {3,option3}
        };
    }

    private void Start()
    {
        _uiManager = FindObjectOfType<Ch1_UIManager>();
        _uiManager.OnButtonClicked += UiDocument_OnButtonClicked;
    }

    private void UiDocument_OnButtonClicked(int option)
    {
       InstantiateEnvironment(option); 
        _uiManager.Hide(); 
    }

    private void InstantiateEnvironment(int option)
    {
        if (!_dictionary.ContainsKey(option)) return;
        
        var environmentGo = Instantiate(environmentPrefab);
        var environment = environmentGo.GetComponent<GenerateMaze>();
        
        environment.SetGenerationOption(GenerateMaze.GenerationOption.FROM_SCRIPTABLE);
        environment.SetMazeDataRef(_dictionary[option]);
    }
}
