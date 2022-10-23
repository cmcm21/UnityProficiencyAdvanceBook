using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(MazeData))]
public class MazeDataEditor : Editor
{
    private MazeData _mazeData;
    private SerializedProperty _worldMapSizeProperty;
    private SerializedProperty _worldMapSquareProperty;
    private SerializedProperty _terrainTypeXTextureProperty;
    private bool _drawWorldMap = false;

    private void OnEnable()
    {
        _worldMapSizeProperty = serializedObject.FindProperty("worldMapSize");
        _worldMapSquareProperty = serializedObject.FindProperty("worldMapSquare");
        _terrainTypeXTextureProperty = serializedObject.FindProperty("terrainEnumXTextures");
        
        _mazeData = target as MazeData;
        _drawWorldMap = WorldDrawed();
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.Update();
        DrawPrimitivesFields();
        GUILayout.Label("If you modify values, Clear world map is needed");
        DrawWorldButton();
        //Needed to update properties' values
        serializedObject.ApplyModifiedProperties();
    }
    
    private void DrawPrimitivesFields()
    {
        EditorGUILayout.PropertyField(_worldMapSizeProperty);
        EditorGUILayout.PropertyField(_worldMapSquareProperty);
        EditorGUILayout.PropertyField(_terrainTypeXTextureProperty);
        if(GUILayout.Button("Update textures"))
           _mazeData.UpdateTerrainDict();
    }

    private void DrawWorldButton()
    {
        if (!_drawWorldMap)
        {
            _drawWorldMap = GUILayout.Button("Generate word map");
            if(_drawWorldMap)
                DrawWorldMap();    
        }
        else
        {
            if (GUILayout.Button("Clear world map"))
            {
                _drawWorldMap = false;
                _mazeData.ResetWordMap();
            }
            DrawWorldMap();    
        }       
    }

    private void DrawWorldMap()
    {
        if (_mazeData.WorldMapSize == 0 || _mazeData.WorldMapSquare == 0)
        {
            Debug.LogWarning($"[{GetType()}]:: world map size or world map square is 0");
            return;
        }
        EditorGUILayout.Space();
        var wordMapStyle = GUI.skin.label;
        wordMapStyle.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Word Map",wordMapStyle);
        
        InitDefaultWorldMap();    
        
        var wordMapSquare = _mazeData.WorldMapSquare;
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
             using (new GUILayout.HorizontalScope()) 
                 for (int i = 0; i < _mazeData.WorldMap.Count; i += wordMapSquare) 
                     using (new GUILayout.VerticalScope())
                         for (int j = i; j < i + wordMapSquare; j++)
                             DrawWorldMapTile(j);
    }

    private void DrawWorldMapTile(int index)
    {
        using (new GUILayout.VerticalScope())
        {
            var terrainType = _mazeData.WorldMap[index];
            var texture = _mazeData.terrainEnumTexDict[terrainType];

            var content = new GUIContent(texture);
            content.tooltip = terrainType.ToString();
            
            var buttonClicked = GUILayout.Button(
                content,
                GUILayout.Height(60),
                GUILayout.Width(50),
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true)
                );

            //var option = (TerrainType)EditorGUILayout.EnumPopup(_mazeData.WorldMap[index]);
            if (buttonClicked)
                _mazeData.UpdateWorldMapNext(index, terrainType);
        }
    }

    private void InitDefaultWorldMap()
    {
        if (_mazeData.WorldMap.Count > 0) return;
        var defaultWorldMap = Enumerable.Repeat(TerrainType.GROUND, _mazeData.WorldMapSize).ToList(); 
        _mazeData.SetWordList(defaultWorldMap);
    }

    private bool WorldDrawed()
    {
        return _mazeData.WorldMap.Count > 0 && _mazeData.terrainEnumTexDict.Count > 0;
    }
}
