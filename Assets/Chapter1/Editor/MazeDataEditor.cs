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
    private bool _drawWorldMap = false;

    private void OnEnable()
    {
        _worldMapSizeProperty = serializedObject.FindProperty("worldMapSize");
        _worldMapSquareProperty = serializedObject.FindProperty("worldMapSquare");
        
        _mazeData = target as MazeData;
        _drawWorldMap = _mazeData.WorldMap.Count != 0;
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
        serializedObject.ApplyModifiedProperties();
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
            Color color = _mazeData.WorldMap[index] == TerrainType.WALL
                ? Color.gray
                : (Color)new Color32(194, 104, 35, 255);

            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(70), GUILayout.Height(70));
            EditorGUI.DrawRect(rect, color);

            var option = (TerrainType)EditorGUILayout.EnumPopup(_mazeData.WorldMap[index]);
            _mazeData.UpdateWorldMap(index, option);
        }
    }

    private void InitDefaultWorldMap()
    {
        if (_mazeData.WorldMap.Count > 0) return;
        var defaultWorldMap = Enumerable.Repeat(TerrainType.GROUND, _mazeData.WorldMapSize).ToList(); 
        _mazeData.SetWordList(defaultWorldMap);
    }
}
