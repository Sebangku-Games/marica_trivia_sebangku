using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class GameUtility 
{
    public const float ResolutionDelayTime = 1;
    public const string SavePrefKey = "Game_Highscore_Value";

    public const string fileName = "Pertanyaan";

    public static string fileDir
    {
        get
        {
            return Application.dataPath + "/";
        }
    }
}

[System.Serializable()]
public class Data
{
    public Pertanyaan[] pertanyaans = new Pertanyaan[0];

    public Data() { }

    public static void Write(Data data, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }

    public static Data Fetch(string filePath)
    {
        return Fetch(out bool result, filePath);
    }

    public static Data Fetch(out bool result, string filePath)
    {
        if(!File.Exists(filePath)) { result = false;  return new Data(); }
        XmlSerializer deserializer = new XmlSerializer(typeof(Data));
        using (Stream stream = new FileStream(filePath, FileMode.Open))
        {
            var data = (Data)deserializer.Deserialize(stream);
            result = true;
            return data;
        }
    }
}
