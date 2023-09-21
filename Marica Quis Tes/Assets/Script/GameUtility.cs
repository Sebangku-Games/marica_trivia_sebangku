using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

public class GameUtility 
{
    public const float ResolutionDelayTime = 2;
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

    public static Data Fetch(TextAsset xmlAsset)
    {
        Data data = new Data();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);

        XmlNodeList pertanyaanNodes = xmlDoc.SelectNodes("Data/pertanyaans/Pertanyaan");
        List<Pertanyaan> questions = new List<Pertanyaan>();

        foreach (XmlNode pertanyaanNode in pertanyaanNodes)
        {
            string pertanyaanInfo = pertanyaanNode.SelectSingleNode("Info").InnerText;
            bool useTimer = bool.Parse(pertanyaanNode.SelectSingleNode("UseTimer").InnerText);
            int timer = int.Parse(pertanyaanNode.SelectSingleNode("Timer").InnerText);
            AnswerType type = pertanyaanNode.SelectSingleNode("Type").InnerText == "Single" 
                ? AnswerType.Single
                : AnswerType.Multi;
            int addScore = int.Parse(pertanyaanNode.SelectSingleNode("AddScore").InnerText); ;

            XmlNodeList jawabanNodes = pertanyaanNode.SelectNodes("Answers/Jawaban");
            List<Jawaban> answers = new List<Jawaban>();

            foreach (XmlNode jawabanNode in jawabanNodes)
            {
                string jawabanInfo = jawabanNode.SelectSingleNode("Info").InnerText;
                bool isCorrect = bool.Parse(jawabanNode.SelectSingleNode("IsCorrect").InnerText);

                var answer = new Jawaban();
                answer.IsCorrect = isCorrect;
                answer.Info = jawabanInfo;

                answers.Add(answer);
            }

            var question = new Pertanyaan();
            question.Info = pertanyaanInfo;
            question.Answers = answers.ToArray();
            question.Timer = timer;
            question.AddScore = addScore;
            question.Type = type;
            question.UseTimer = useTimer;

            questions.Add(question);
        }

        data.pertanyaans = questions.ToArray();
        return data;

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
