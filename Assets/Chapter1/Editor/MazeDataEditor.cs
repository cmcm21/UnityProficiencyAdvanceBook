using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeData))]
public class MazeDataEditor : Editor
{
    private MazeData _mazeData;
    private SerializedProperty _worldMapSizeProperty;
    private SerializedProperty _worldMapSquareProperty;
    private bool _drawWorldMap = false;

    private void OnEnable()
    {
        _worldMapSizeProperty = serializedObject.FindProperty("worldMapSize");
        _worldMapSquareProperty = serializedObject.FindProperty("worldMapSquare");
        
        _mazeData = target as MazeData;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPrimitivesFields();
        if (!_drawWorldMap)
        {
            _drawWorldMap = GUILayout.Button("Generate word map");
            if(_drawWorldMap)
                DrawWorldMap();    
        }
        else
            DrawWorldMap();    
    }

    private void DrawPrimitivesFields()
    {
        EditorGUILayout.PropertyField(_worldMapSizeProperty);
        EditorGUILayout.PropertyField(_worldMapSquareProperty);
    }

    private void DrawWorldMap()
    {
        if (_mazeData.WorldMapSize == 0 || _mazeData.WorldMapSquare == 0)
        {
            Debug.LogError($"[{GetType()}]:: world map size or world map square is 0");
            return;
        }
        EditorGUILayout.Space();
        InitDefaultWorldMap();    
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) {
             using (new GUILayout.HorizontalScope()) {
                 var wordMapSquare = _mazeData.WorldMapSquare;
                 for (int i = 0; i < _mazeData.WorldMap.Count; i += wordMapSquare) {
                     using (new GUILayout.VerticalScope()) {
                         for (int j = i; j < i + wordMapSquare; j++) {
                             var option = (TerrainType)EditorGUILayout.EnumPopup(_mazeData.WorldMap[j]);
                             _mazeData.UpdateWorldMap(j,option);
                         }
                     }
                 }           
             }           
        }
    }

    private void InitDefaultWorldMap()
    {
        if (_mazeData.WorldMap.Count > 0) return;
        var defaultWorldMap = Enumerable.Repeat(TerrainType.GROUND, _mazeData.WorldMapSize).ToList(); 
        _mazeData.SetWordList(defaultWorldMap);
    }
}
