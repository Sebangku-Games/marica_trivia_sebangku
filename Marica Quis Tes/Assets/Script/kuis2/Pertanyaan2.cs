using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Jawaban2
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } }
}

[CreateAssetMenu(fileName = "bukux2", menuName = "Quiz/Pertanyaan2")]
public class Pertanyaan2 : ScriptableObject
{
    public enum AnswerType { Multi, Single }

    [SerializeField] private string _info = string.Empty;
    public string Info { get { return _info; } }

    [SerializeField] Jawaban2[] _answers = null;
    public Jawaban2[] Answers { get { return _answers; } }

    //Parameters

    [SerializeField] private bool _useTimer = false;
    public bool UseTimer { get { return _useTimer; } }

    [SerializeField] private int _timer = 0;
    public int Timer { get { return _timer; } }

    [SerializeField] private AnswerType _answerType = AnswerType.Multi;
    public AnswerType GetAnswerType { get { return _answerType; } }

    [SerializeField] private int _addScore = 10;
    public int AddScore { get { return _addScore; } }

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
