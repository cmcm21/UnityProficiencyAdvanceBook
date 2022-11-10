#nullable enable
using System;
using System.Xml;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MazeXMLData
{
    private XmlDocument _xmlDocument;
    
    private int _mapSize;
    public int MapSize => _mapSize;
    
    private int _mapSquare;
    public int MapSquare => _mapSquare;

    private int _tileSize;
    public int TileSize => _tileSize;
    
    private Vector3 _location;
    public Vector3 Location => _location;
    
    private Vector3 _scale;
    public Vector3 Scale => _scale;
    
    private Color _color;
    public Color Color => _color;

    private bool _initialized;
    public bool Initialized => _initialized;

    public MazeXMLData(XmlDocument xmlDocument)
    {
        _xmlDocument = xmlDocument;

        var objectNode = xmlDocument.SelectSingleNode("maze/object");
        _initialized = true;
        if (objectNode != null && objectNode.Attributes != null)
        {
            _mapSize = GetValue<int>("mapSize",objectNode)!.IntValue;
            _mapSquare = GetValue<int>("mapSquare",objectNode)!.IntValue;
            _tileSize = GetValue<int>("wallSize",objectNode)!.IntValue;
            _location = GetValue<Vector3>("wallLocation",objectNode)!.Vector3Value;
            _scale = GetValue<Vector3>("wallScale",objectNode)!.Vector3Value;
            _color = GetValue<Color>("wallColor",objectNode)!.ColorValue;
        }
        else
        {
            Debug.LogError($"[{GetType()}]:: node object was not found on xml file ");
            _initialized = false;
        }
        
        Debug.Log($"[{GetType()}]:: object : {GetObjectString()}");
    }

    private ValueConverter? GetValue<T>(string xmlAttribute,XmlNode xmlNode)
    {
        var xmlProperty = xmlNode.Attributes?.GetNamedItem(xmlAttribute);
        if (xmlProperty != null)
        {
            ValueConverter converterValue = new ValueConverter();
            converterValue.SetValue<T>(xmlProperty.Value);
            
            return converterValue;
        }
        Debug.LogError($"[{GetType()}]:: id {xmlAttribute} was not found on xml file ");
        _initialized = false;
        return null;
    }

    private string GetObjectString()
    {
        return $"mapSize: {_mapSize}, mapSquare: {_mapSquare}, Location: {_location}, " +
               $"scale: {_scale}, tileSize: {_tileSize}, color: {_color.ToString()}, initialized :{_initialized}";
    }
}

public class ValueConverter
{
    private int _intValue;
    public int IntValue => _intValue;
    
    private Vector3 _vectorValue;
    public Vector3 Vector3Value => _vectorValue;

    private Color _colorValue;
    public Color ColorValue => _colorValue;

    public void SetValue<T>(string value)
    {
        if (typeof(T) == typeof(int))
            _intValue = Convert.ToInt32(value);
        else if (typeof(T) == typeof(Vector3))
            _vectorValue = ToVector(value);
        else if (typeof(T) == typeof(Color))
            _colorValue = ToColor(value);
    }
    
     private Vector3 ToVector(string vectorString)
     {
         var values = vectorString.Split(',');
         
         return new Vector3(
             float.Parse(values[0]),
             float.Parse(values[1]),
             float.Parse(values[2])
         );
     }
 
     private Color ToColor(string colorString)
     {
         try
         {
             var values = colorString.Split(',');
             return new Color32(
                 (byte)Convert.ToInt16(values[0]),
                 (byte)Convert.ToInt16(values[1]),
                 (byte)Convert.ToInt16(values[2]),
                 255
             );
         }
         catch(Exception e)
         {
             return Color.gray;
         }
     }   
}