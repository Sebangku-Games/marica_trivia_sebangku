using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnswerType { Multi, Single }

[System.Serializable()]
public class Jawaban
{
    public string Info = string.Empty;
    public bool IsCorrect = false;

    public Jawaban() { }
}
[System.Serializable()]
public class Pertanyaan
{


    public string Info = null;
    public Jawaban[] Answers = null;
    public bool UseTimer = false;
    public int Timer = 0;
    public AnswerType Type = AnswerType.Single;
    public int AddScore = 0;

    public Pertanyaan() { }


    /// <summary>
    /// Function that is called to collect and return correct answers indexes.
    /// </summary>
    public List<int> GetCorrectAnswers()
    {
        List<int> CorrectAnswers = new List<int>();
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers.Add(i);
            }
        }
        return CorrectAnswers;
    }
}
